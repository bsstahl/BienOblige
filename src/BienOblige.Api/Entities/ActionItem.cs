using BienOblige.Api.Converters;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class ActionItem
{
    public static string[] DefaultObjectTypeName = new string[] { "bienoblige:ActionItem", "Object" };


    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    public List<KeyValuePair<string?, string>> Context { get; set; } = Constants.Context.Default;

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("@type")]
    public List<string> ObjectTypeName { get; set; } = DefaultObjectTypeName.ToList();


    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }


    [JsonPropertyName("attributedTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? AttributedTo { get; set; }

    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("updated")]
    public DateTimeOffset LastUpdatedAt { get; set; }

    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    [JsonPropertyName("published")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? Published { get; set; }

    [JsonPropertyName("generator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Actor? Generator { get; set; }

    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IActionItemTarget? Target { get; set; }

    [JsonPropertyName("bienoblige:parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Parent { get; set; }

    [JsonPropertyName("bienoblige:completionMethods")]
    [JsonConverter(typeof(EnumListConverter<CompletionMethod>))]
    public List<CompletionMethod> CompletionMethods { get; set; } = new List<CompletionMethod>();

    //[JsonPropertyName("location")]
    //public NetworkObjectCollection? Location { get; set; }

    //[JsonPropertyName("tag")]
    //public NetworkObjectCollection? Tags { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();
}
