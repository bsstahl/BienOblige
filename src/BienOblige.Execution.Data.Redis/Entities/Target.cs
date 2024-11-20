using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Redis.Entities;

public class Target
{
    [JsonPropertyName("@type")]
    public required string ObjectType { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonExtensionData]
    public IDictionary<string, string> ExtendedProperties = new Dictionary<string, string>();
}
