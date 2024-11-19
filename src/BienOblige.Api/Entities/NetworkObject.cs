using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class NetworkObject
{
    [JsonPropertyName("@type")]
    public string ObjectType { get; set; }

    [JsonPropertyName("id")]
    public string ObjectId { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();

    public NetworkObject(string objectId, string objectType)
    {
        this.ObjectId = objectId;
        this.ObjectType = objectType;
    }
}
