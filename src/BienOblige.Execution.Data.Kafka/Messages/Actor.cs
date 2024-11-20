using System.Text.Json.Serialization;
using BienOblige.Execution.Data.Kafka.Extensions;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Actor
{
    [JsonPropertyName("@type")]
    public required string @Type { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }


    public ActivityStream.Aggregates.Actor AsAggregate()
    {
        return new ActivityStream.Builders.ActorBuilder()
            .Id(ActivityStream.ValueObjects.NetworkIdentity.From(this.Id))
            .ActorType(this.Type.AsActorType())
            .Build();
    }

    public static Actor From(ActivityStream.Aggregates.Actor actor)
    {
        return new Actor()
        { 
            Id = actor.Id.Value.ToString(),
            @Type = actor.ObjectTypeName.Single().ToString()
        };
    }

}
