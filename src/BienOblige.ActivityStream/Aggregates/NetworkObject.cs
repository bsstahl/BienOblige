using BienOblige.ActivityStream.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.ActivityStream.Aggregates;

public class NetworkObject(NetworkIdentity id, TypeName objectTypeName)
{
    [JsonPropertyName("id")]
    public NetworkIdentity Id { get; set; } = id;

    [JsonPropertyName("@type")]
    public TypeName ObjectTypeName { get; set; } = objectTypeName;

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();
}
