using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Context
{
    public NamespaceKey? Key { get; set; }
    public NamespaceName Name { get; set; }

    public bool HasKey => Key is not null;

    public KeyValuePair<string, string> AsKeyValuePair()
        => new KeyValuePair<string, string>(Key?.Value ?? string.Empty, Name.Value);

    public Context(string name, string? key = null)
    {
        Name = NamespaceName.From(name);
        Key = key is null
            ? null
            : NamespaceKey.From(key);
    }

    public Context(JsonElement element)
    {
        // TODO: Add better validation
        if (element.ValueKind.Equals(JsonValueKind.String))
        {
            Name = NamespaceName.From(element.GetString());
            Key = null;
        }
        else
        {
            var e = element.EnumerateObject().First();
            Name = NamespaceName.From(e.Value.ToString());
            Key = NamespaceKey.From(e.Name.ToString());
        }
    }

    public override bool Equals(object obj)
    {
        return obj is Context other
            ? HasKey == other.HasKey &&
                   Key == other.Key &&
                   Name == other.Name
            : false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(HasKey, Key, Name);
    }

    //public override string ToString()
    //{
    //    return this.Key is null
    //        ? this.Name.Value
    //        : $"\"{this.Key}\": \"{this.Name}\"";
    //}
}
