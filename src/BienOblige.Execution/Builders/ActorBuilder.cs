using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Enumerations;
using BienOblige.Execution.ValueObjects;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Builders;

public class ActorBuilder
{
    private NetworkIdentity? _id;
    private ActorType? _type;
    private ActorName? _name;

    public Actor Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_type, nameof(_type));
        return new Actor(_id, _type.Value, _name);
    }

    public ActorBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ActorBuilder Id(string value)
    {
        return this.Id(NetworkIdentity.From(value));
    }

    public ActorBuilder Id(NetworkIdentity value)
    {
        _id = value;
        return this;
    }

    public ActorBuilder ActorType(ActorType value)
    {
        _type = value;
        return this;
    }

    public ActorBuilder Name(string value)
    {
        return this.Name(ActorName.From(value));
    }

    public ActorBuilder Name(ActorName value)
    {
        _name = value;
        return this;
    }
}
