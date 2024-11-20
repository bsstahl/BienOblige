using BienOblige.Execution.Data.Kafka.Extensions;
using BienOblige.ActivityStream.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class ActionItem
{
    [JsonConstructor]
    public ActionItem(string id, string name, string content)
    {
        Id = id;
        Name = name;
        Content = content;
    }

    //public ActionItem(JsonElement element)
    //{
    //    this.Id = element.GetStringProperty("id");
    //    this.Name = element.GetStringProperty(nameof(Name).ToLower());
    //    this.Content = element.GetStringProperty(nameof(Content).ToLower());

    //    if (element.TryGetProperty(nameof(Target).ToLower(), out var targetElement))
    //        this.Target = Target.Parse(targetElement);
    //}

    [JsonPropertyName("@type")]
    public string[] ObjectType { get; private set; } = new[] { "bienoblige:ActionItem", "Object" };

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("target")]
    public Target? Target { get; set; }


    public ActivityStream.Aggregates.ActionItem AsAggregate()
    {
        return new ActivityStream.Aggregates.ActionItem(
            NetworkIdentity.From(this.Id),
            ActivityStream.ValueObjects.Name.From(this.Name),
            ActivityStream.ValueObjects.Content.From(this.Content));
    }

    public static ActionItem From(ActivityStream.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString(), item.Name.Value, item.Content.Value)
        { };
    }
}

