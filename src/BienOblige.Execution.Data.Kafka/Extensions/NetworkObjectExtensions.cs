using BienOblige.ActivityStream.Aggregates;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Extensions;

public static class NetworkObjectExtensions
{
    public static bool IsActionItem(this NetworkObject value)
        => value.ObjectTypeName.Select(t => t.Value)
            .Contains(Constants.TypeName.ActionItem);

    public static NetworkObject? GetTarget(this NetworkObject value)
    {
        return value.AdditionalProperties.TryGetValue(Constants.FieldName.Target, out var element)
            ? JsonSerializer.Deserialize<NetworkObject>(element.GetRawText())
            : null;
    }
}
