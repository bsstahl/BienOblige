using BienOblige.Api.Entities;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class ObjectIdentifierBuilder
{
    private Uri? _id;
    private List<string> _objectTypes = new();
    private List<KeyValuePair<string?, string>>? _context;
    private string? _name;
    private readonly Dictionary<string, object> _additionalProperties = new();

    public NetworkObject Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));

        return !_objectTypes.Any()
            ? throw new InvalidOperationException("An object type must be specified.")
            : new NetworkObject
            {
                Id = _id,
                ObjectType = _objectTypes,
                Context = _context,
                Name = _name,
                AdditionalProperties = _additionalProperties
            };
    }

    public ObjectIdentifierBuilder AddContext(string? key, string value)
    {
        return this.AddContext([new KeyValuePair<string?, string>(key, value)]);
    }

    public ObjectIdentifierBuilder AddContext(IEnumerable<KeyValuePair<string?, string>>? values)
    {
        if (values?.Any() ?? false)
        {
            _context ??= new();
            _context.AddRange(values);
        }
        return this;
    }

    public ObjectIdentifierBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public ObjectIdentifierBuilder Id(Guid id, string entityTypeName)
    {
        return this.Id($"{Constants.Path.DefaultBaseUri}/{entityTypeName}/{id.ToString()}");
    }

    public ObjectIdentifierBuilder Id(string id)
    {
        return this.Id(new Uri(id));
    }

    public ObjectIdentifierBuilder Id(NetworkIdentity id)
    {
        return this.Id(id.Value);
    }

    public ObjectIdentifierBuilder Id(Uri id)
    {
        _id = id;
        return this;
    }

    public ObjectIdentifierBuilder AddObjectType(string value)
    {
        _objectTypes.Add(value);
        return this;
    }

    public ObjectIdentifierBuilder AddObjectTypes(IEnumerable<string>? values)
    {
        _objectTypes.AddRange(values ?? Array.Empty<string>());
        return this;
    }

    public ObjectIdentifierBuilder ClearObjectTypes()
    {
        _objectTypes.Clear();
        return this;
    }

    public ObjectIdentifierBuilder Name(string? value)
    {
        _name = value;
        return this;
    }

    public ObjectIdentifierBuilder AddAdditionalProperties(IDictionary<string, object> values)
    {
        values.ToList().ForEach(v => this.AddAdditionalProperty(v.Key, v.Value));
        return this;
    }

    public ObjectIdentifierBuilder AddAdditionalProperty(string key, object? value)
    {
        if (value is not null)
            _additionalProperties.Add(key, value);
        return this;
    }

}