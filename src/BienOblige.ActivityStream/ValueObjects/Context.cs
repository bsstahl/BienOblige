using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class Context : ValueOf<IEnumerable<ContextItem>, Context>
{
    public override string ToString()
    {
        return $"[{string.Join(",", this.Value.OrderBy(v => v.Value.Key).ToString())}]";
    }

    public static Context Default => Context.From(new List<ContextItem>
    {
        ContextItem.From("https://www.w3.org/ns/activitystreams"),
        ContextItem.From("bienoblige", "https://bienoblige.com/ns"),
        ContextItem.From("schema", "https://schema.org")
    });
}
