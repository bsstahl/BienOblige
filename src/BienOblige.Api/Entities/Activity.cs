﻿using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Activity
{
    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    public List<KeyValuePair<string?, string>> Context { get; set; } = new();

    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    [JsonPropertyName("bienoblige:correlationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? CorrelationId { get; set; }

    [JsonPropertyName("@type")]
    public required string ActivityType { get; set; }

    [JsonPropertyName("actor")]
    public required Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public required NetworkObject Object { get; set; }

    [JsonPropertyName("target")]
    public NetworkObject? Target { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();

}
