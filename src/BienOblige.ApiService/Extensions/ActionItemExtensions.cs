namespace BienOblige.ApiService.Extensions;

public static class ActionItemExtensions
{
    internal static IEnumerable<Execution.Aggregates.ActionItem> AsAggregates(this IEnumerable<Entities.ActionItem> actionItems)
    {
        return actionItems.Select(actionItem => actionItem.AsAggregate());
    }

    public static JsonContent AsJsonContent(this Execution.Aggregates.ActionItem actionItem)
    {
        return JsonContent.Create(new
        {
            Id = actionItem.Id.Value,
            Name = actionItem.Title.Value,
            Content = actionItem.Content.Value
        });
    }
}
