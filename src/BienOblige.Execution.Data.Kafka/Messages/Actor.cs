using System.Text.Json.Serialization;
using System.Text.Json;
using BienOblige.Execution.Data.Kafka.Extensions;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Actor
{
    public Actor(string id, string @type)
    {
        Id = id;
        Type = @type;
    }

    internal Actor(JsonElement actorNode)
        : this(actorNode.GetStringProperty("id"), actorNode.GetStringProperty("@type"))
    { }

    [JsonPropertyName("@type")]
    public string @Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }


    public Aggregates.Actor AsAggregate()
    {
        return Aggregates.Actor.From(this.Id, this.Type);
    }

    public static Actor From(Aggregates.Actor actor)
    {
        return new Actor(actor.Id.Value.ToString(), actor.Type.ToString());
    }

}
