using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Context: List<ContextItem>
{
    public Context(JsonElement element)
    {
        // TODO: Add better validation
        if (element.ValueKind.Equals(JsonValueKind.String))
            this.Add(new ContextItem(element.GetString()));
        else
            this.AddRange(element.EnumerateObject().Select(e => new ContextItem(e.Value.ToString(), e.Name)));
    }

    public Context(IEnumerable<ContextItem> items)
    {
        this.AddRange(items);
    }

    public static Context From(ActivityStream.ValueObjects.Context context)
    {
        return new Context(context.Value.Select(c => new ContextItem(c.Value.Value, c.Value.Key)));
    }

}
