using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Activity
{
    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    public List<KeyValuePair<string?, string>> Context { get; set; } = new();

    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    [JsonPropertyName("bienoblige:correlationId")]
    public required Uri CorrelationId { get; set; }

    [JsonPropertyName("@type")]
    public required string ActivityType { get; set; }

    [JsonPropertyName("actor")]
    public required Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public required ActionItem ActionItem { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    //[JsonConstructor]
    //public Activity(Uri Id, ActivityType @type, Actor actor,
    //    ActionItem @object, DateTimeOffset? published = null)
    //{
    //    this.CorrelationId = Id;
    //    this.ActivityType = @type.ToString();
    //    this.Actor = actor;
    //    this.ActionItem = @object;
    //    this.Published = published ?? DateTimeOffset.UtcNow;
    //}
}
