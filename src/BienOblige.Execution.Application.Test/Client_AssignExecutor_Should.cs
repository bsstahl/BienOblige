using BienOblige.ValueObjects;
using BienOblige.Execution.Application.Test.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using BienOblige.Execution.Application.Extensions;
using Microsoft.Extensions.Logging;
using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_AssignExecutor_Should
{
    IServiceProvider _services;

    public Client_AssignExecutor_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .UseExecutionClient()
            .UseMockRepositories()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();
    }

    [Fact]
    public async Task ThrowIfNoActionItemIdSupplied()
    {
        NetworkIdentity? actionItemId = null;
        NetworkIdentity? executorId = (null as NetworkIdentity).CreateRandom();
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var correlationId = Guid.NewGuid().ToString();
        var updatingActor = Actor.From(updatingActorId, "Person");

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.AssignExecutor(actionItemId!, executorId, updatingActor, correlationId));
    }

    [Fact]
    public async Task ThrowIfNoExecutorIdIsSupplied()
    {
        var actionItemId = (null as NetworkIdentity).CreateRandom();
        NetworkIdentity? executorId = null;
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = Actor.From(updatingActorId, "Person");
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.AssignExecutor(actionItemId, executorId!, updatingActor, correlationId));
    }

    [Fact]
    public async Task ThrowIfNoUpdatingActorIsSupplied()
    {
        var actionItemId = (null as NetworkIdentity).CreateRandom();
        var executorId = (null as NetworkIdentity).CreateRandom();
        Actor? updatingActor = null;
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.AssignExecutor(actionItemId, executorId, updatingActor!, correlationId));
    }

}