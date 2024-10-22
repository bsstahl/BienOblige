using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BienOblige.AppHost.Kafka;

public class HealthCheckAnnotation(Func<IResource, CancellationToken, Task<IHealthCheck?>> healthCheckFactory) : IResourceAnnotation
{
    public Func<IResource, CancellationToken, Task<IHealthCheck?>> HealthCheckFactory { get; } = healthCheckFactory;

    public static HealthCheckAnnotation Create(Func<string, IHealthCheck> connectionStringFactory)
    {
        return new(async (resource, token) =>
        {
            return resource is not IResourceWithConnectionString c
                ? null
                : await c.GetConnectionStringAsync(token) is not string cs
                    ? null
                    : connectionStringFactory(cs);
        });
    }
}

internal class WaitOnAnnotation(IResource resource) : IResourceAnnotation
{
    public IResource Resource { get; } = resource;
    public string[]? States { get; set; }
    public bool WaitUntilCompleted { get; set; }
}
