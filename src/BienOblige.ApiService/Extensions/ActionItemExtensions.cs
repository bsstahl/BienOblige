using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActionItemExtensions
{
    internal static IEnumerable<ActivityStream.Aggregates.NetworkObject> AsAggregates(this IEnumerable<NetworkObject> actionItems)
    {
        return actionItems.Select(actionItem => actionItem.AsAggregate());
    }

    public static ActivityStream.Aggregates.NetworkObject AsAggregate(this ActionItem actionItem)
    {
        ArgumentNullException.ThrowIfNull(actionItem.Id, nameof(actionItem.Id));

        return new ActivityStream.Builders.ObjectBuilder()
            .Id(ActivityStream.ValueObjects.NetworkIdentity.From(actionItem.Id))
            .Name(actionItem.Name)
            .Content(ActivityStream.ValueObjects.Content.From(actionItem.Content))
            // .Generator(actionItem.Generator?.AsAggregate())
            // .Target(actionItem.Target?.AsAggregate())
            // .Parent(actionItem.Parent is not null ? ActivityStream.ValueObjects.NetworkIdentity.From(actionItem.Parent) : null)
            .Build();
    }

}
