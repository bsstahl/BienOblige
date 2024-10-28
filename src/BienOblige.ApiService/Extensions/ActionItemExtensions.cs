using BienOblige.Execution.ValueObjects;

namespace BienOblige.ApiService.Extensions;

public static class ActionItemExtensions
{
    public static JsonContent AsJsonContent(this Execution.Aggregates.ActionItem actionItem)
    {
        return JsonContent.Create(new
        {
            Id = actionItem.Id.Value,
            Title = actionItem.Title.Value
        });
    }
}
