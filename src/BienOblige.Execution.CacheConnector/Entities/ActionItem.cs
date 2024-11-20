using System.Text.Json.Serialization;
using System.Text.Json;

namespace BienOblige.Execution.CacheConnector.Entities;

public class ActionItem
{
    [JsonPropertyName("@type")]
    public required string[] ObjectTypeName { get; set; } = new[] { "bienoblige:ActionItem", "Object" };

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
            ObjectTypeName = item.ObjectTypeName.Select(t => t.Value).ToArray(),
            Target = item.Target is not null
                ? new Target()
                {
                    ObjectTypeName = item.Target.ObjectTypeName.Select(n => n.Value).ToArray(),
                    Id = item.Target.Id.Value.ToString(),
                    Name = item.Target.Name?.Value
                }
                : null
        };
    }
}
