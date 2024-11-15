using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Target
{
    public Target(string objectType, string id, string name, string description)
    {
        ObjectType = objectType;
        Id = id;
        Name = name;
        Description = description;
    }

    internal static Target? Parse(JsonElement element)
    {
        if (element.ValueKind.Equals(JsonValueKind.Null))
            return null;
        else
        {
            string? objectType = null;
            string? objectId = null;
            string? objectName = null;
            string? objectDescription = null;
            var extendedProperties = new Dictionary<string, string>();

            foreach (var property in element.EnumerateObject())
            {
                var name = property.Name;
                var val = property.Value.GetString();
                switch (name)
                {
                    case "@type":
                        objectType = val;
                        break;
                    case "@id":
                        objectId = val;
                        break;
                    case "name":
                        objectName = val;
                        break;
                    case "description":
                        objectDescription = val;
                        break;
                    default:
                        extendedProperties.Add(name, val);
                        break;
                }
            }

            return new Target(objectType, objectId, objectName, objectDescription)
            {
                ExtendedProperties = extendedProperties
            };
        }
    }

    [JsonPropertyName("@type")]
    public string ObjectType { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    // [JsonExtensionData]
    public IDictionary<string, string> ExtendedProperties = new Dictionary<string, string>();
}
