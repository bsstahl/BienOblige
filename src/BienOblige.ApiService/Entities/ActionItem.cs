using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.ApiService.Entities;

public class ActionItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("generator")]
    public Actor? Generator { get; set; }

    [JsonPropertyName("target")]
    public NetworkObject? Target { get; set; }

    [JsonPropertyName("bienoblige:parent")]
    public NetworkObject? Parent { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();

    public ActionItem(string id, string name, string content)
    {
        this.Id = id;
        this.Name = name;
        this.Content = content;
    }

    public ActivityStream.Aggregates.ActionItem AsAggregate()
    {
        ArgumentNullException.ThrowIfNull(this.Id, nameof(this.Id));

        return new ActivityStream.Aggregates.ActionItem(
            ActivityStream.ValueObjects.NetworkIdentity.From(this.Id),
            ActivityStream.ValueObjects.Name.From(this.Name),
            ActivityStream.ValueObjects.Content.From(this.Content))
        {
            Generator = this.Generator?.AsAggregate(),
            Target = this.Target?.AsAggregate(),
            Parent = this.Parent?.AsAggregate()
        };
    }

    public static ActionItem From(ActivityStream.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString(), item.Name.Value, item.Content.Value);
    }

}
