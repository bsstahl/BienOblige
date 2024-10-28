using System.Text.Json.Serialization;
using System.Text.Json;
using BienOblige.Execution.Data.Kafka.Extensions;

namespace BienOblige.Execution.Data.Kafka.Aggregates;

public class Actor
{
    public Actor(string id, string @type)
    {
        this.Id = id;
        this.Type = @type;
    }

    internal Actor(JsonElement actorNode)
        : this(actorNode.GetStringProperty("id"), actorNode.GetStringProperty("@type"))
    { }

    [JsonPropertyName("@type")]
    public string @Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}
