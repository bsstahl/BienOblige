using System.Text.Json;
using ValueOf;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Context : ValueOf<List<ContextItem>, Context>
{
    public static Context From(JsonElement element)
    {
        // TODO: Add better validation
        var items = new List<ContextItem>();
        if (element.ValueKind.Equals(JsonValueKind.String))
            items.Add(new ContextItem(element.GetString()));
        else
            items.AddRange(element.EnumerateObject().Select(e => new ContextItem(e.Value.ToString(), e.Name)));
        return Context.From(items);
    }

    public static Context From(ActivityStream.ValueObjects.Context context)
    {
        var items = new List<ContextItem>();
        context.Value.ToList().ForEach(c => items.Add(new ContextItem(c.Value.Value, c.Value.Key)));
        return Context.From(items);
    }

}
