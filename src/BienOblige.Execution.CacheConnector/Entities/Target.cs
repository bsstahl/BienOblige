using System.Text.Json.Serialization;

namespace BienOblige.Execution.CacheConnector.Entities;

public class Target
{
    [JsonPropertyName("@type")]
    public required string[] ObjectTypeName { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    // [JsonExtensionData]
    public IDictionary<string, string> ExtendedProperties = new Dictionary<string, string>();
}
