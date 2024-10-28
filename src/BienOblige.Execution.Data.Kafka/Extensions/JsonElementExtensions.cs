using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Extensions;

internal static class JsonElementExtensions
{
    internal static string GetStringProperty(this JsonElement element, string propertyName)
        => element.GetProperty(propertyName).GetString()!;

    internal static IEnumerable<ValueObjects.Context> ParseContext(this JsonElement contextNode)
    {
        var result = new List<ValueObjects.Context>();
        foreach (var node in contextNode.EnumerateArray())
        {
            if (node.ValueKind == JsonValueKind.String)
            {
                result.Add(new ValueObjects.Context(node.GetString()));
            }
            else if (node.ValueKind == JsonValueKind.Array)
            {
                foreach (var child in node.EnumerateArray())
                {
                    var e = child.EnumerateObject().First();
                    result.Add(new ValueObjects.Context(e.Value.ToString(), e.Name.ToString()));
                }
            }
            else if (node.ValueKind == JsonValueKind.Object)
            {
                foreach(var child in node.EnumerateObject())
                    result.Add(new ValueObjects.Context(child.Value.GetString(), child.Name));
            }
            else
            {
                throw new NotImplementedException($"Invalid context node type {node.ValueKind}");
            }
        }

        return result;
    }
}
