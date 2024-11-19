using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActorExtensions
{
    public static ActivityStream.Aggregates.Actor AsAggregate(this Actor actor)
    {
        return new ActivityStream.Aggregates.Actor(
            ActivityStream.ValueObjects.NetworkIdentity.From(actor.Id),
            actor.ActorType.AsActorType());
    }

    public static Actor From(this ActivityStream.Aggregates.Actor actor)
    {
        return actor.Id.From(Enum.Parse<ActivityStream.Enumerations.ActorType>(actor.ObjectTypeName.Value));
    }

    public static Actor From(this ActivityStream.ValueObjects.NetworkIdentity actorId,
        ActivityStream.Enumerations.ActorType actorType)
    {
        return new Actor(actorId.Value.ToString(), actorType.ToString());
    }

}
