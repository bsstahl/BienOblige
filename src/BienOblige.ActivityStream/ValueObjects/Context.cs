using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class Context : ValueOf<IEnumerable<ContextItem>, Context>
{
    public override string ToString()
    {
        return $"[{string.Join(",", this.Value.OrderBy(v => v.Value.Key).ToString())}]";
    }

    public static Context Empty => Context.From(new List<ContextItem>());
}
