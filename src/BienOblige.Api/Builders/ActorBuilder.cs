using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActorBuilder
{
    private Uri? _id;
    private ActorType? _actorType;
    private string? _name;

    private List<KeyValuePair<string?, string>>? _context;
    private Dictionary<string, object> _additionalProperties = new();

    public Actor Build()
    {
        ArgumentNullException.ThrowIfNull(_id);
        ArgumentNullException.ThrowIfNull(_actorType);

        return new Actor()
        { 
            Id = _id.ToString(),
            ActorType = _actorType.Value.ToString(),
            Context = _context,
            Name = _name,
            AdditionalProperties = _additionalProperties
        };
    }

    public ActorBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public ActorBuilder AddContext(string? key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public ActorBuilder AddContext(IEnumerable<KeyValuePair<string?, string>>? context)
    {
        if (context?.Any() ?? false)
        {
            _context ??= new();
            _context.AddRange(context);
        }
        return this;
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
