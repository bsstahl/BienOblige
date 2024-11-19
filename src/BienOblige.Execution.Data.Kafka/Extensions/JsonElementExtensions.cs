using BienOblige.Execution.Data.Kafka.Messages;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Extensions;

internal static class JsonElementExtensions
{
    internal static string GetStringProperty(this JsonElement element, string propertyName)
        => element.GetProperty(propertyName).GetString()!;

    internal static IEnumerable<ContextItem> ParseContext(this JsonElement contextNode)
    {
        var result = new List<ContextItem>();
        foreach (var node in contextNode.EnumerateArray())
        {
            if (node.ValueKind == JsonValueKind.String)
            {
                result.Add(new ContextItem(node.GetString()));
            }
            else if (node.ValueKind == JsonValueKind.Array)
            {
                foreach (var child in node.EnumerateArray())
                {
                    var e = child.EnumerateObject().First();
                    result.Add(new ContextItem(e.Value.ToString(), e.Name.ToString()));
                }
            }
            else if (node.ValueKind == JsonValueKind.Object)
            {
                foreach(var child in node.EnumerateObject())
                    result.Add(new ContextItem(child.Value.GetString(), child.Name));
            }
            else
            {
                throw new NotImplementedException($"Invalid context node type {node.ValueKind}");
            }
        }

        return result;
    }
}
