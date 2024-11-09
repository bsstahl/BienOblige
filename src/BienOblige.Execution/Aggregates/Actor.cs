using BienOblige.Execution.Enumerations;
using BienOblige.Execution.ValueObjects;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Aggregates;

public class Actor
{
    public NetworkIdentity Id { get; set; }
    public ActorType Type { get; set; }
    public ActorName? Name { get; set; }

    public Actor(NetworkIdentity id, ActorType type, ActorName? name = null)
    {
        this.Id = id;
        this.Type = type;
        this.Name = name;
    }

    public static Actor From(string actorId, string actorType)
    {
        return new Actor(NetworkIdentity.From(actorId), Enum.Parse<ActorType>(actorType));
    }
}
