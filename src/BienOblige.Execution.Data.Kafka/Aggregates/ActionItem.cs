using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Aggregates;

public class ActionItem
{
    // TODO: Make the following properties optional
    // actionItemContent
    // targetType, targetId, targetName, targetDescription

    public ActionItem(string id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public ActionItem(JsonElement element)
    {
        this.Id = element.GetStringProperty("@id");
        this.Name = element.GetStringProperty(nameof(this.Name).ToLower());
        this.Content = element.GetStringProperty(nameof(this.Content).ToLower());
        if (element.TryGetProperty(nameof(this.Target).ToLower(), out var targetElement))
            this.Target = new Target(targetElement);
    }

    [JsonPropertyName("@type")]
    public string[] ObjectType { get; private set; } = new[] { "bienoblige:ActionItem", "Object" };

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("target")]
    public Target? Target { get; set; }
}

