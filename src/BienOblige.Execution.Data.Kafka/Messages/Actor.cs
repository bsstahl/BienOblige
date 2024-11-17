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


    public ActivityStream.Aggregates.Actor AsAggregate()
    {
        return new ActivityStream.Aggregates.Actor(
            ActivityStream.ValueObjects.NetworkIdentity.From(this.Id), 
            this.Type.AsActorType());
    }

    public static Actor From(ActivityStream.Aggregates.Actor actor)
    {
        return new Actor(actor.Id.Value.ToString(), actor.ObjectTypeName.Value.ToString());
    }

}
