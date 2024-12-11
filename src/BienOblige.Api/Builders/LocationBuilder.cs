using BienOblige.Api.Entities;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class LocationBuilder
{
    private Uri? _id;
    private string? _name;

    private List<KeyValuePair<string?, string>>? _context;
    private Dictionary<string, object> _additionalProperties = new();

    // TODO: Add additional fields

    public Place Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));

        return new Place()
        {
            Id = _id.ToString(),
            Name = _name,
            Context = _context,
            AdditionalProperties = _additionalProperties
        };
    }

    public LocationBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public LocationBuilder AddContext(string? key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public LocationBuilder AddContext(IEnumerable<KeyValuePair<string?, string>>? context)
    {
        if (context?.Any() ?? false)
        {
            _context ??= new();
            _context.AddRange(context);
        }
        return this;
    }

    public LocationBuilder Id(string id)
    {
        return this.Id(new Uri(id));
    }

    public LocationBuilder Id(NetworkIdentity id)
    {
        return this.Id(id.Value);
    }

    public LocationBuilder Id(Uri id)
    {
        _id = id;
        return this;
    }

    public LocationBuilder Name(string value)
    {
        _name = value;
        return this;
    }
}
