using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Extensions;
using BienOblige.Execution.Application.Test.Mocks;
using BienOblige.ActivityStream.Builders;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using BienOblige.ActivityStream.Enumerations;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_PublishActivityCommand_Should
{
    IServiceProvider _services;

    public Client_PublishActivityCommand_Should(ITestOutputHelper outputHelper)
    {
        _services = new ServiceCollection()
            .UseExecutionClient()
            .UseMockRepositories()
            .AddLogging(b => b.AddXUnit(outputHelper))
            .BuildServiceProvider();
    }

    [Fact]
    public async Task ThrowIfNoActivityIsSupplied()
    {
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActorType.Person);

        var activity = null as Activity;

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.PublishActivityCommand(activity!));
    }

    [Fact]
    public async Task ThrowIfNoActionItemIsSupplied()
    {
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId),
            ActorType.Person);

        var activity = new Activity(NetworkIdentity.New(), ActivityType.Update,
            updatingActor, null!, DateTimeOffset.UtcNow);

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(()
            => target.PublishActivityCommand(activity));
    }

    [Fact]
    public async Task ThrowIfNoUpdatingActorIsSupplied()
    {
        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();

        var correlationId = Guid.NewGuid().ToString();
        var activity = new Activity(NetworkIdentity.New(), ActivityType.Update,
            null!, item, DateTimeOffset.UtcNow);

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.PublishActivityCommand(activity));
    }

    [Fact]
    public async Task SuccessfullyCreateTheActivity()
    {
        var updatingActorId = (null as NetworkIdentity).CreateRandom().Value.ToString();
        var updatingActor = new Actor(
            NetworkIdentity.From(updatingActorId), 
            ActorType.Service);

        var activityType = ActivityType.Create;
        var item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = NetworkIdentity.From(Guid.NewGuid());

        var mockRepo = _services.GetRequiredService<IPublishActivityCommands>() as MockActivityCreator;
        mockRepo!.SetupCreateActivities(activityType, item, updatingActor, correlationId);

        var activity = new Activity(correlationId, activityType,
            updatingActor, item, DateTimeOffset.UtcNow);

        var target = _services.GetRequiredService<Client>();
        var id = await target.PublishActivityCommand(activity);

        mockRepo!.VerifyAll();
    }

}