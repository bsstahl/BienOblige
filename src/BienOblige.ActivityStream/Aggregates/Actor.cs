using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Aggregates;

public class Actor
{
    public NetworkIdentity Id { get; set; }
    public ActorType @Type { get; set; }
    public ActorName? Name { get; set; }

    public Actor(NetworkIdentity id, ActorType type, ActorName? name = null)
    {
        this.Id = id;
        this.@Type = type;
        this.Name = name;
    }
}
