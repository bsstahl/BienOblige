using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActorExtensions
{
    public static ActivityStream.Aggregates.Actor AsAggregate(this Actor actor)
    {
        return new ActivityStream.Builders.ActorBuilder()
            .Id(ActivityStream.ValueObjects.NetworkIdentity.From(actor.Id))
            .ActorType(actor.ActorType.AsActorType())
            .Build();
    }

    public static Actor From(this ActivityStream.Aggregates.Actor actor)
    {
        return actor.Id.From(Enum.Parse<ActivityStream.Enumerations.ActorType>(actor.ObjectTypeName.Single().Value));
    }

    public static Actor From(this ActivityStream.ValueObjects.NetworkIdentity actorId,
        ActivityStream.Enumerations.ActorType actorType)
    {
        return new Actor()
        {
            Id = actorId.Value.ToString(),
            ActorType = actorType.ToString()
        };
    }

}
