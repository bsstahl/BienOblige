using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Exceptions;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Extensions;
using BienOblige.Execution.Application.Test.Mocks;
using BienOblige.Execution.Builders;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using BienOblige.ActivityStream.Enumerations;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_CreateActionItem_Should
{
    IServiceProvider _services;

    public Client_CreateActionItem_Should(ITestOutputHelper outputHelper)
    {
        _services = new ServiceCollection()
            .UseExecutionClient()
            .UseMockRepositories()
            .AddLogging(b => b.AddXUnit(outputHelper))
            .BuildServiceProvider();
    }

    [Fact]
    public async Task ThrowIfNoActionItemIsSupplied()
    {
        List<ActionItem> items = new();
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActivityStream.Enumerations.ActorType.Person);
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentException>(() 
            => target.CreateActionItem(items, updatingActor, correlationId));
    }

    [Fact]
    public async Task ThrowIfNoActionItemListIsSupplied()
    {
        List<ActionItem>? items = null;
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActivityStream.Enumerations.ActorType.Person);
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(()
            => target.CreateActionItem(items!, updatingActor, correlationId));
    }


    [Fact]
    public async Task ThrowIfNoUpdatingActorIsSupplied()
    {
        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        Actor? updatingUser = null;
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.CreateActionItem(new[] { item }, updatingUser!, correlationId));
    }

    [Fact]
    public async Task ThrowIfTheUpdatingActorIdIsInvalid()
    {
        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<InvalidIdentifierException>(()
            => target.CreateActionItem(new[] { item },
                new Actor(
                    NetworkIdentity.From(string.Empty.GetRandom()),
                    ActivityStream.Enumerations.ActorType.Person),
                correlationId));
    }

    [Fact]
    public async Task SuccessfullyCreateTheActivity()
    {
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActivityStream.Enumerations.ActorType.Person);
        var item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var mockRepo = _services.GetRequiredService<ICreateActivities>() as MockActivityCreator;
        mockRepo!.SetupCreateActivities(ActivityType.Create, new[] { item }, updatingActor, correlationId);

        var target = _services.GetRequiredService<Client>();
        var id = await target.CreateActionItem(new[] { item }, updatingActor, correlationId);

        mockRepo!.VerifyAll();
    }
}