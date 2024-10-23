using Confluent.Kafka;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Create
{
    [JsonPropertyName("@context")]
    public string Context { get; private set; } = "https://www.w3.org/ns/activitystreams";

    [JsonPropertyName("type")]
    public string ActivityType { get; private set; } = "Create";

    [JsonPropertyName("id")]
    public string CorrelationId { get; set; }

    [JsonPropertyName("actor")]
    public Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public ActionItem ActionItem { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    public Create(string correlationId, DateTimeOffset published, string actionItemId, string actionItemName, 
        string actionItemContent, string targetType, string targetId, string targetName, string targetDescription, string actorId)
    {
        this.CorrelationId = correlationId;
        this.Published = published;
        this.Actor = new Actor(actorId);
        this.ActionItem = new ActionItem(actionItemId, actionItemName, actionItemContent);
    }
}

public class ActionItem(string id, string name, string content)
{
    [JsonPropertyName("type")]
    public string ObjectType { get; private set; } = "ActionItem";

    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("name")]
    public string Name { get; set; } = name;

    [JsonPropertyName("content")]
    public string Content { get; set; } = content;
}

public class Target(string objectType, string id, string name, string description)
{
    [JsonPropertyName("type")]
    public string ObjectType { get; set; } = objectType;

    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("name")]
    public string Name { get; set; } = name;

    [JsonPropertyName("description")]
    public string Description { get; set; } = description;
}

public class Actor(string id)
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Person";

    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
}
