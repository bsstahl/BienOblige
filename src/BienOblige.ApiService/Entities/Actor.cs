using BienOblige.ApiService.Extensions;
using System.Text.Json.Serialization;

namespace BienOblige.ApiService.Entities;

public class Actor(string id, string actorType)
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("@type")]
    public string ActorType { get; set; } = actorType;

    [JsonPropertyName("name")]
    public string? Name { get; set; }


    public BienOblige.ActivityStream.Aggregates.Actor AsAggregate()
    {
        return new ActivityStream.Aggregates.Actor(
            ActivityStream.ValueObjects.NetworkIdentity.From(this.Id), 
            this.ActorType.AsActorType());
    }

    public static Actor From(BienOblige.ActivityStream.Aggregates.Actor actor)
    {
        return Actor.From(actor.Id, actor.@Type);
    }

    public static Actor From(
        ActivityStream.ValueObjects.NetworkIdentity actorId, 
        ActivityStream.Enumerations.ActorType actorType)
    {
        return new Actor(actorId.Value.ToString(), actorType.ToString());
    }
}
