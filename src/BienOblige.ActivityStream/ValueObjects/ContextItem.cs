using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class ContextItem : ValueOf<KeyValuePair<string?, string>, ContextItem>
{
    public override string ToString()
    {
        return Value.Key is null 
            ? $"\"{Value.Value}\"" 
            : $"{{ \"{Value.Key}\": \"{Value.Value}\" }}";
    }

    public static ContextItem From(string value)
        => ContextItem.From(null, value);

    public static ContextItem From(string? key, string value) 
        => ContextItem.From(new KeyValuePair<string?, string>(key, value));

}
