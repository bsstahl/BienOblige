using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.ValueObjects;

public class Context
{
    public NamespaceKey? Key { get; set; }
    public NamespaceName Name { get; set; }

    public bool HasKey => this.Key is not null;

    public KeyValuePair<string, string> AsKeyValuePair()
        => new KeyValuePair<string, string>(this.Key?.Value ?? string.Empty, this.Name.Value);

    public Context(string name, string? key = null)
    {
        this.Name = NamespaceName.From(name);
        this.Key = key is null
            ? null
            : NamespaceKey.From(key);
    }

    public Context(JsonElement element)
    {
        // TODO: Add better validation
        if (element.ValueKind.Equals(JsonValueKind.String))
        {
            this.Name = NamespaceName.From(element.GetString());
            this.Key = null;
        }
        else
        {
            var e = element.EnumerateObject().First();
            this.Name = NamespaceName.From(e.Value.ToString());
            this.Key = NamespaceKey.From(e.Name.ToString());
        }
    }

    //public override string ToString()
    //{
    //    return this.Key is null
    //        ? this.Name.Value
    //        : $"\"{this.Key}\": \"{this.Name}\"";
    //}
}
