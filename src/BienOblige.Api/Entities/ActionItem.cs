using BienOblige.Api.Enumerations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class ActionItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("@type")]
    public string[] ObjectTypeName { get; set; } = new[] { "bienoblige:ActionItem", "Object" };

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("attributedTo")]
    public NetworkObject? AttributedTo { get; set; }

    //[JsonPropertyName("@context")]
    //public Context Context { get; set; } = Context.Default;

    [JsonPropertyName("endTime")]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("updated")]
    public DateTimeOffset LastUpdatedAt { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    [JsonPropertyName("generator")]
    public Actor? Generator { get; set; }

    [JsonPropertyName("target")]
    public NetworkObject? Target { get; set; }

    [JsonPropertyName("bienoblige:parent")]
    public string? Parent { get; set; }

    [JsonPropertyName("bienoblige:completionMethods")]
    public IEnumerable<CompletionMethod> CompletionMethods { get; set; } = new List<CompletionMethod>();

    //[JsonPropertyName("location")]
    //public NetworkObjectCollection? Location { get; set; }

    //[JsonPropertyName("tag")]
    //public NetworkObjectCollection? Tags { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();


    public ActionItem(string id, string name, string content)
    {
        this.Id = id;
        this.Name = name;
        this.Content = content;
    }

}
