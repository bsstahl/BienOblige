using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActorBuilder
{
    private Uri? _id;
    private ActorType? _actorType;
    private string? _name;

    public Actor Build()
    {
        ArgumentNullException.ThrowIfNull(_id);
        ArgumentNullException.ThrowIfNull(_actorType);

        return new Actor(_id.ToString(), _actorType.Value.ToString())
        {
            Name = _name
        };
    }

    public ActorBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ActorBuilder Id(string value)
    {
        return this.Id(new Uri(value));
    }

    public ActorBuilder Id(Uri value)
    {
        _id = value;
        return this;
    }

    public ActorBuilder ActorType(ActorType value)
    {
        _actorType = value;
        return this;
    }

    public ActorBuilder Name(string value)
    {
        _name = value;
        return this;
    }
}
