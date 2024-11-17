namespace BienOblige.ApiService.Extensions;

public static class ActionItemExtensions
{
    internal static IEnumerable<ActivityStream.Aggregates.ActionItem> AsAggregates(this IEnumerable<Entities.ActionItem> actionItems)
    {
        return actionItems.Select(actionItem => actionItem.AsAggregate());
    }

    public static JsonContent AsJsonContent(this ActivityStream.Aggregates.ActionItem actionItem)
    {
        return JsonContent.Create(new
        {
            Id = actionItem.Id.Value,
            Name = actionItem.Name.Value,
            Content = actionItem.Content.Value
        });
    }
}
