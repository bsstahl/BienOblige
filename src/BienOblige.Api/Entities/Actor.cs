using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Actor
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("@type")]
    public required string ActorType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
