using BienOblige.ActivityStream.ValueObjects;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class ActionItem
{
    [JsonPropertyName("@type")]
    public string[] ObjectType { get; private set; } = new[] { "bienoblige:ActionItem", "Object" };

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("target")]
    public Target? Target { get; set; }


    public ActivityStream.Aggregates.ActionItem AsAggregate()
    {
        return new ActivityStream.Aggregates.ActionItem()
        {
            Id = NetworkIdentity.From(this.Id),
            Name = ActivityStream.ValueObjects.Name.From(this.Name),
            Content = ActivityStream.ValueObjects.Content.From(this.Content),
            ObjectTypeName = this.ObjectType.Select(t => TypeName.From(t))
        };
    }

    public static ActionItem From(ActivityStream.Aggregates.ActionItem item)
    {
        return new ActionItem()
        {
            Id = item.Id.Value.ToString(),
            Name = item.Name?.Value ?? string.Empty,
            Content = item.Content?.Value ?? string.Empty,
            ObjectType = item.ObjectTypeName.Select(t => t.Value).ToArray()
        };
    }
}

