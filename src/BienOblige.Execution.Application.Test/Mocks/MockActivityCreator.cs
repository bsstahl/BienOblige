using BienOblige.Execution.Application.Interfaces;
using BienOblige.ActivityStream.ValueObjects;
using Microsoft.Extensions.Logging;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.Enumerations;

namespace BienOblige.Execution.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActivityCreator(ILogger<MockActivityCreator> logger, IServiceProvider services) : IPublishActivityCommands
{
    private readonly ILogger _logger = logger;
    private readonly IServiceProvider _services = services;
    private readonly Mock<IPublishActivityCommands> _activitiesCreator = new();

    public async Task<NetworkIdentity> Publish(Activity activity)
    {
        return await _activitiesCreator.Object.Publish(activity);
    }

    internal void SetupCreateActivities(ActivityType activityType, ActionItem actionItem, Actor updatingActor, NetworkIdentity correlationId)
    {
        _activitiesCreator
            .Setup(x => x.Publish(It.IsAny<Activity>()))
            .Returns(Task.FromResult(actionItem.Id))
            .Callback<Activity>(p =>
                {
                    if (!activityType.Equals(p.ActivityType))
                        throw new ArgumentException($"Incorrect ActivityType: Expected '{activityType}' but got '{p.ActivityType}'");

                    if (!actionItem.Id.Equals(p.ActionItem.Id))
                        throw new ArgumentException($"Incorrect ActionItem Id: Expected '{actionItem.Id}' but got '{p.ActionItem.Id}'");

                    if (!correlationId.Equals(p.CorrelationId))
                        throw new ArgumentException($"Incorrect Correlation Id: Expected '{correlationId}' but got '{p.Id}'");

                    if (!updatingActor.Id.Equals(p.Actor.Id))
                        throw new ArgumentException($"Incorrect Actor Id: Expected '{updatingActor.Id}' but got '{p.Actor.Id}'");
                })
            .Verifiable(Times.Once);
    }

    internal void VerifyAll()
    {
        _activitiesCreator.VerifyAll();
    }
}
