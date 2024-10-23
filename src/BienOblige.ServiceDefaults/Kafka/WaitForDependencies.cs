using Aspire.Hosting.Lifecycle;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace BienOblige.ServiceDefaults.Kafka;

internal class WaitForDependenciesRunningHook(
       DistributedApplicationExecutionContext executionContext,
        ResourceNotificationService resourceNotificationService) :
        IDistributedApplicationLifecycleHook,
        IAsyncDisposable
{
    private readonly CancellationTokenSource _cts = new();

    public Task BeforeStartAsync(DistributedApplicationModel appModel,
        CancellationToken cancellationToken = default)
    {
        if (executionContext.IsPublishMode)
        {
            return Task.CompletedTask;
        }

        var waitingResources = new ConcurrentDictionary<IResource, ConcurrentDictionary<WaitOnAnnotation, TaskCompletionSource>>();
        foreach (var resource in appModel.Resources)
        {
            var waitOnAnnotation = resource.Annotations.OfType<WaitOnAnnotation>().ToLookup(r => r.Resource);
            resource.Annotations.Add(
                new EnvironmentCallbackAnnotation(
                    async context => await DependenciesWaitAsync(
                        context,
                        waitingResources,
                        resource,
                        waitOnAnnotation)));
        }

        _ = Task.Run(this.ExecuteStateChange(waitingResources), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task DependenciesWaitAsync(
        EnvironmentCallbackContext context,
        ConcurrentDictionary<IResource, ConcurrentDictionary<WaitOnAnnotation, TaskCompletionSource>> waitingResources,
        IResource currentResource,
        ILookup<IResource, WaitOnAnnotation> waitOnResources)
    {

        var dependencies = new List<Task>();
        foreach (var group in waitOnResources)
        {
            var resource = group.Key;
            if (resource == currentResource) continue;
            var pendingAnnotations = waitingResources.GetOrAdd(resource, _ => new());
            foreach (var waitOn in group)
            {
                var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

                async Task Wait()
                {
                    context.Logger?.LogInformation("Waiting for {Resource}.", waitOn.Resource.Name);

                    await tcs.Task;

                    context.Logger?.LogInformation("Waiting for {Resource} completed.", waitOn.Resource.Name);
                }
                pendingAnnotations[waitOn] = tcs;

                dependencies.Add(Wait());
            }
        }

        await resourceNotificationService.PublishUpdateAsync(currentResource, s => s with
        {
            State = new("Waiting", KnownResourceStateStyles.Info)
        });

        await Task.WhenAll(dependencies).WaitAsync(context.CancellationToken);
    }

    private Func<Task?> ExecuteStateChange(
        ConcurrentDictionary<IResource, ConcurrentDictionary<WaitOnAnnotation, TaskCompletionSource>>
            waitingResources)
        => async () =>
        {
            var stoppingToken = _cts.Token;
            static bool IsKnownTerminalState(CustomResourceSnapshot state) =>
                state.State == "FailedToStart" ||
                state.State == "Exited" ||
                state.ExitCode is not null;

            await foreach (var resourceEvent in resourceNotificationService.WatchAsync(stoppingToken))
            {
                if (waitingResources.TryGetValue(resourceEvent.Resource, out var pendingAnnotations))
                {
                    foreach (var (waitOn, tcs) in pendingAnnotations)
                    {
                        if (waitOn.States is { } states && states.Contains(resourceEvent.Snapshot.State?.Text,
                                StringComparer.Ordinal))
                        {
                            pendingAnnotations.TryRemove(waitOn, out _);
                            _ = DoTheHealthCheck(resourceEvent, tcs, _cts.Token);
                        }
                        else if (waitOn.WaitUntilCompleted)
                        {
                            if (IsKnownTerminalState(resourceEvent.Snapshot))
                            {
                                pendingAnnotations.TryRemove(waitOn, out _);
                                _ = DoTheHealthCheck(resourceEvent, tcs, _cts.Token);
                            }
                        }
                        {
                            if (resourceEvent.Snapshot.State?.Text == "Running")
                            {
                                pendingAnnotations.TryRemove(waitOn, out _);
                                _ = DoTheHealthCheck(resourceEvent, tcs, _cts.Token);
                            }
                            else if (IsKnownTerminalState(resourceEvent.Snapshot))
                            {
                                pendingAnnotations.TryRemove(waitOn, out _);

                                tcs.TrySetException(new Exception($"Dependency {waitOn.Resource.Name} failed to start"));
                            }
                        }
                    }
                }
            }

        };

    private async Task DoTheHealthCheck(ResourceEvent resourceEvent, TaskCompletionSource tcs, CancellationToken cancellationToken)
    {
        var resource = resourceEvent.Resource;
        HealthCheckAnnotation? healthCheckAnnotation = null;

        // Find the relevant health check annotation. If the resource has a parent, walk up the tree
        // until we find the health check annotation.
        while (true)
        {
            // If we find a health check annotation, break out of the loop
            if (resource.TryGetLastAnnotation(out healthCheckAnnotation))
            {
                break;
            }

            // If the resource has a parent, walk up the tree
            if (resource is IResourceWithParent parent)
            {
                resource = parent.Parent;
            }
            else
            {
                break;
            }
        }

        var operation = await BuildHealthCheckOperation(cancellationToken, healthCheckAnnotation, resource, tcs);
        try
        {
            if (operation is not null)
            {
                var pipeline = this.CreateResiliencyPipeline();

                await pipeline.ExecuteAsync(operation, cancellationToken);
            }

            tcs.TrySetResult();
        }
        catch (Exception ex)
        {

            tcs.TrySetException(ex);
        }
    }

    private ResiliencePipeline CreateResiliencyPipeline()
    {
        var retryUntilCancelled = new RetryStrategyOptions()
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            BackoffType = DelayBackoffType.Exponential,
            MaxRetryAttempts = 5,
            UseJitter = true,
            MaxDelay = TimeSpan.FromSeconds(30)
        };

        return new ResiliencePipelineBuilder().AddRetry(retryUntilCancelled).Build();
    }

    private static async Task<Func<CancellationToken, ValueTask>?> BuildHealthCheckOperation(CancellationToken cancellationToken,
        HealthCheckAnnotation? healthCheckAnnotation, IResource resource, TaskCompletionSource tcs)
    {
        Func<CancellationToken, ValueTask>? operation = null;
        if (healthCheckAnnotation?.HealthCheckFactory is { } factory)
        {
            IHealthCheck? check;
            try
            {
                check = await factory(resource, cancellationToken);
                if (check is not null)
                {
                    var context = new HealthCheckContext()
                    {
                        Registration = new HealthCheckRegistration("", check, HealthStatus.Unhealthy, [])
                    };

                    operation = async (cancellationToken) =>
                    {
                        var result = await check.CheckHealthAsync(context, cancellationToken);

                        if (result.Exception is not null)
                        {
                            ExceptionDispatchInfo.Throw(result.Exception);
                        }

                        if (result.Status != HealthStatus.Healthy)
                        {
                            throw new Exception("Health check failed");
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        return operation;
    }

    public ValueTask DisposeAsync()
    {
        _cts.Cancel();
        return default;
    }
}
