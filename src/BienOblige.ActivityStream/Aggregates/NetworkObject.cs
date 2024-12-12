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

    [JsonPropertyName("attachment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObjectCollection? Attachment { get; set; }

    [JsonPropertyName("attributedTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? AttributedTo { get; set; }

    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Content? Content { get; set; }

    [JsonPropertyName("@context")]
    public Context Context { get; set; } = Context.Empty;

    [JsonPropertyName("generator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Generator { get; set; }

    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObjectCollection? Location { get; set; }

    [JsonPropertyName("name")]
    public Name? Name { get; set; }

    [JsonPropertyName("published")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? Published { get; set; }

    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObjectCollection? Tags { get; set; }

    [JsonPropertyName("updated")]
    public DateTimeOffset LastUpdatedAt { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();



    //private NetworkObject? audience { get; set; }
    //private NetworkObject? endTime { get; set; }
    //private NetworkObject? icon { get; set; }
    //private NetworkObject? image { get; set; }
    //private NetworkObject? inReplyTo { get; set; }
    //private NetworkObject? preview { get; set; }
    //private NetworkObject? replies { get; set; }
    //private NetworkObject? startTime { get; set; }
    //private NetworkObject? url { get; set; }
    //private NetworkObject? to { get; set; }
    //private NetworkObject? bto { get; set; }
    //private NetworkObject? cc { get; set; }
    //private NetworkObject? bcc { get; set; }
    //private NetworkObject? mediaType { get; set; }
    //private NetworkObject? duration { get; set; }

}
