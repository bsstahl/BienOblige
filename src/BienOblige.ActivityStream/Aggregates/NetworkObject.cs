using BienOblige.ActivityStream.Collections;
using BienOblige.ActivityStream.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.ActivityStream.Aggregates;

public class NetworkObject
{
    [JsonPropertyName("id")]
    public required NetworkIdentity Id { get; set; }

    [JsonPropertyName("@type")]
    public required IEnumerable<TypeName> ObjectTypeName { get; set; }

    [JsonPropertyName("attributedTo")]
    public NetworkObject? AttributedTo { get; set; }

    [JsonPropertyName("content")]
    public Content? Content { get; set; }

    [JsonPropertyName("@context")]
    public Context Context { get; set; } = Context.Default;

    [JsonPropertyName("generator")]
    public NetworkObject? Generator { get; set; }

    [JsonPropertyName("location")]
    public NetworkObjectCollection? Location { get; set; }

    [JsonPropertyName("name")]
    public Name? Name { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("tag")]
    public NetworkObjectCollection? Tags { get; set; }

    [JsonPropertyName("updated")]
    public DateTimeOffset LastUpdatedAt { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();

}
