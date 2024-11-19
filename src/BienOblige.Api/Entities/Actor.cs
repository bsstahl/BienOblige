using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Actor(string id, string actorType)
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("@type")]
    public string ActorType { get; set; } = actorType;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
