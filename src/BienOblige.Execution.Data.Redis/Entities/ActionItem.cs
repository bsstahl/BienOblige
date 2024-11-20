using System.Text.Json.Serialization;
using System.Text.Json;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Data.Redis.Extensions;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BienOblige.Execution.Data.Redis.Entities;

public class ActionItem
{
    [JsonPropertyName("@type")]
    public required string[] ObjectType { get; set; } = new[] { "bienoblige:ActionItem", "Object" };

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("target")]
    public Target? Target { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public ActivityStream.Aggregates.ActionItem AsAggregate()
    {
        return new ActivityStream.Builders.ActionItemBuilder()
            .Id(this.Id)
            .Name(this.Name)
            .Content(this.Content)
            .Build();
    }

    public static ActionItem From(ActivityStream.Aggregates.ActionItem item)
    {
        return new ActionItem()
        {
            Id = item.Id.Value.ToString(),
            Name = item.Name.Value,
            Content = item.Content.Value,
            Target = item.Target is not null
                ? new Target()
                {
                    ObjectType = item.Target.ObjectTypeName.Single().ToString(),
                    Id = item.Target.Id.Value.ToString(),
                    Name = item.Target.Name.Value
                }
                : null,
            ObjectType = item.ObjectTypeName.Select(t => t.Value).ToArray()
        };
    }

    public static ActionItem Deserialize(string json)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        var deserializationResult = JsonSerializer.Deserialize<ActionItem>(json);
        ArgumentNullException.ThrowIfNull(deserializationResult, nameof(deserializationResult));
        return deserializationResult;
    }
}
