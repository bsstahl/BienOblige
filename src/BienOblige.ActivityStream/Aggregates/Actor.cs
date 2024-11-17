using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Aggregates;

public class Actor : NetworkObject
{
    public Actor(NetworkIdentity id, ActorType actorType, Name? actorName = null)
        : base(id, TypeName.From(actorType))
    {
        this.Name = actorName;
    }
}
