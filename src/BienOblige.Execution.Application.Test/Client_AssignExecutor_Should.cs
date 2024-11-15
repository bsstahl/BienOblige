using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Application.Test.Extensions;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

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
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActivityStream.Enumerations.ActorType.Person);

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
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActivityStream.Enumerations.ActorType.Person);

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