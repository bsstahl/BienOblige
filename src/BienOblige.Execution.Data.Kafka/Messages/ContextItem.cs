namespace BienOblige.Execution.Data.Kafka.Messages;

public class ContextItem
{
    public NamespaceKey? Key { get; set; }
    public NamespaceName Name { get; set; }

    public bool HasKey => Key is not null;

    public ContextItem(string name, string? key = null)
    {
        Name = NamespaceName.From(name);
        Key = key is null
            ? null
            : NamespaceKey.From(key);
    }

    public KeyValuePair<string, string> AsKeyValuePair()
        => new KeyValuePair<string, string>(Key?.Value ?? string.Empty, Name.Value);

    public override bool Equals(object obj)
    {
        return obj is ContextItem other
            ? HasKey == other.HasKey &&
                   Key == other.Key &&
                   Name == other.Name
            : false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(HasKey, Key, Name);
    }

}
