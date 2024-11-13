using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Enumerations;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application.Test.Mocks;

internal class MockActivityCreator(ILogger<MockActivityCreator> logger, IServiceProvider services) : ICreateActivities
{
    private readonly ILogger _logger = logger;
    private readonly Mock<ICreateActivities> _activitiesCreator = new();
    private readonly IServiceProvider _services = services;

    public async Task<IEnumerable<NetworkIdentity>> Create(ActivityType activityType, IEnumerable<ActionItem> items, Actor updatingActor, string correlationId)
    {
        return await _activitiesCreator.Object.Create(activityType, items, updatingActor, correlationId);
    }

    internal void SetupCreateActivities(ActivityType activityType, ActionItem[] actionItems, Actor updatingActor, string correlationId)
    {
        var itemIds = actionItems.Select(i => i.Id);
        var itemIdValues = itemIds.Select(i => i.Value.ToString());
        var uId = updatingActor.Id.Value.ToString();
        var cId = correlationId;

        _activitiesCreator
            .Setup(x => x.Create(It.IsAny<ActivityType>(), It.IsAny<IEnumerable<ActionItem>>(), It.IsAny<Actor>(), It.IsAny<string>()))
            .Returns(Task.FromResult(itemIds))
            .Callback<ActivityType, IEnumerable<ActionItem>, Actor, string>((p_activityType, p_item, p_actor, p_correlationId) =>
                {
                    if (!activityType.Equals(p_activityType))
                        throw new ArgumentException($"Incorrect ActivityType: Expected {activityType} but got {p_activityType}");

                    p_item.ToList().ForEach(p_item =>
                    {
                        if (!itemIdValues.Contains(p_item.Id.Value.ToString()))
                            throw new ArgumentException($"Expected Id {p_item.Id.Value.ToString()} not found in {itemIdValues}");
                    });

                    if (!uId.Equals(p_actor.Id.Value.ToString()))
                        throw new ArgumentException($"Incorrect Updating Actor Id: Expected {uId} but got {p_actor.Id.Value.ToString()}");

                    if (!cId.Equals(p_correlationId))
                        throw new ArgumentException($"Incorrect Correlation Id: Expected {correlationId} but got {p_correlationId}");
                })
            .Verifiable(Times.Once);
    }

    internal void VerifyAll()
    {
        _activitiesCreator.VerifyAll();
    }
}
