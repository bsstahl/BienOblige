using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActionItemExtensions
{
    internal static IEnumerable<ActivityStream.Aggregates.ActionItem> AsAggregates(this IEnumerable<ActionItem> actionItems)
    {
        return actionItems.Select(actionItem => actionItem.AsAggregate());
    }

    public static ActivityStream.Aggregates.ActionItem AsAggregate(this ActionItem actionItem)
    {
        ArgumentNullException.ThrowIfNull(actionItem.Id, nameof(actionItem.Id));

        return new ActivityStream.Aggregates.ActionItem(
            ActivityStream.ValueObjects.NetworkIdentity.From(actionItem.Id),
            ActivityStream.ValueObjects.Name.From(actionItem.Name),
            ActivityStream.ValueObjects.Content.From(actionItem.Content))
        {
            Generator = actionItem.Generator?.AsAggregate(),
            Target = actionItem.Target?.AsAggregate(),
            Parent = actionItem.Parent?.AsAggregate()
        };
    }

}
