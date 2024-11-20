using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Builders;

public class ActorBuilder
{
    private NetworkIdentity? _id;
    private ActorType? _type;
    private Name? _name;

    public Actor Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_type, nameof(_type));
        return new Actor()
        {
            Id = _id,
            ObjectTypeName = [TypeName.From(_type.Value)],
            Name = _name
        };
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

    public ActorBuilder Name(string actorName)
    {
        return this.Name(ValueObjects.Name.From(actorName));
    }

    public ActorBuilder Name(Name actorName)
    {
        _name = actorName;
        return this;
    }
}
