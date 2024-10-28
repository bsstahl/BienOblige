using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Aggregates;

public class Target
{
    public Target(string objectType, string id, string name, string description)
    {
        this.ObjectType = objectType;
        this.Id = id;
        this.Name = name;
        this.Description = description;
    }

    internal Target(JsonElement element)
    {
        foreach (var property in element.EnumerateObject())
        {
            var name = property.Name;
            var val = property.Value.GetString();
            switch (name)
            {
                case "@type":
                    this.ObjectType = val;
                    break;
                case "@id":
                    this.Id = val;
                    break;
                case "name":
                    this.Name = val;
                    break;
                case "description":
                    this.Description = val;
                    break;
                default:
                    this.ExtendedProperties.Add(name, val);
                    break;
            }
        }
    }

    [JsonPropertyName("@type")]
    public string ObjectType { get; set; }

    [JsonPropertyName("@id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public IDictionary<string, string> ExtendedProperties = new Dictionary<string, string>();
}
