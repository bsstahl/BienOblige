using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class AddLocationActivityBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private LocationBuilder? _locationBuilder;
    private ObjectIdentifierBuilder? _targetBuilder;

    private Dictionary<string, object>? _additionalProperties;

    private readonly Uri _instanceBaseUri;

    public AddLocationActivityBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public AddLocationActivityBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public Activity Build()
    {
        ArgumentNullException.ThrowIfNull(_locationBuilder, nameof(_locationBuilder));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));
        ArgumentNullException.ThrowIfNull(_targetBuilder, nameof(_targetBuilder));

        // Assign default values where needed
        _activityType = ActivityType.Add;
        _context ??= Constants.Context.Default;
        _correlationId ??= new Uri($"{_instanceBaseUri}/Activity/{Guid.NewGuid()}");
        _published ??= DateTimeOffset.UtcNow;

        return new Activity()
        {
            Id = NetworkIdentity.From(_instanceBaseUri.ToString(),
                nameof(Activity), Guid.NewGuid().ToString()).Value,
            Context = _context,
            CorrelationId = _correlationId,
            ActivityType = _activityType.Value.ToString(),
            Actor = _actorBuilder.Build(),
            Object = _locationBuilder.Build().AsNetworkObject(),
            Target = _targetBuilder.Build().AsNetworkObject(),
            Published = _published,
            AdditionalProperties = _additionalProperties ?? new()
        };
    }

    public AddLocationActivityBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public AddLocationActivityBuilder AddContext(string key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public AddLocationActivityBuilder AddContext(IEnumerable<KeyValuePair<string?, string>> context)
    {
        _context ??= new();
        _context.AddRange(context);
        return this;
    }

    public AddLocationActivityBuilder CorrelationId(Guid value)
    {
        return this.CorrelationId($"urn:uid:{value.ToString()}");
    }

    public AddLocationActivityBuilder CorrelationId(string value)
    {
        return this.CorrelationId(new Uri(value));
    }

    public AddLocationActivityBuilder CorrelationId(Uri value)
    {
        _correlationId = value;
        return this;
    }

    public AddLocationActivityBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public AddLocationActivityBuilder Published(DateTimeOffset? value)
    {
        _published = value;
        return this;
    }

    public AddLocationActivityBuilder Location(LocationBuilder value)
    {
        _locationBuilder = value;
        return this;
    }

    public AddLocationActivityBuilder AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var kvp in additionalProperties)
            this.AddAdditionalProperty(kvp);
        return this;
    }

    public AddLocationActivityBuilder AddAdditionalProperty(string key, object value)
    {
        var kvp = new KeyValuePair<string, object>(key, value);
        return this.AddAdditionalProperty(kvp);
    }

    public AddLocationActivityBuilder AddAdditionalProperty(KeyValuePair<string, object> kvp)
    {
        _additionalProperties ??= new();
        _additionalProperties.Add(kvp.Key, kvp.Value);
        return this;
    }

    public AddLocationActivityBuilder ClearAdditionalProperties()
    {
        _additionalProperties = null;
        return this;
    }

    public AddLocationActivityBuilder Target(ObjectIdentifierBuilder value)
    {
        _targetBuilder = value;
        return this;
    }

}
