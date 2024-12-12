using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class UpdateStatusActivityBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ObjectIdentifierBuilder? _targetBuilder;
    private ObjectBuilder? _statusBuilder;

    private Dictionary<string, object>? _additionalProperties;

    private readonly Uri _instanceBaseUri;

    public UpdateStatusActivityBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public UpdateStatusActivityBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public Activity Build()
    {
        ArgumentNullException.ThrowIfNull(_statusBuilder, nameof(_statusBuilder));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));
        ArgumentNullException.ThrowIfNull(_targetBuilder, nameof(_targetBuilder));

        // Assign default values where needed
        _activityType = ActivityType.Update;
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
            Object = _statusBuilder.Build(),
            Target = _targetBuilder.Build().AsNetworkObject(),
            Published = _published,
            AdditionalProperties = _additionalProperties ?? new()
        };
    }

    public UpdateStatusActivityBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public UpdateStatusActivityBuilder AddContext(string key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public UpdateStatusActivityBuilder AddContext(IEnumerable<KeyValuePair<string?, string>> context)
    {
        _context ??= new();
        _context.AddRange(context);
        return this;
    }

    public UpdateStatusActivityBuilder CorrelationId(Guid value)
    {
        return this.CorrelationId($"urn:uid:{value.ToString()}");
    }

    public UpdateStatusActivityBuilder CorrelationId(string value)
    {
        return this.CorrelationId(new Uri(value));
    }

    public UpdateStatusActivityBuilder CorrelationId(Uri value)
    {
        _correlationId = value;
        return this;
    }

    public UpdateStatusActivityBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public UpdateStatusActivityBuilder Published(DateTimeOffset? value)
    {
        _published = value;
        return this;
    }

    public UpdateStatusActivityBuilder Status(Status value)
    {
        Uri statusUri = new Uri($"{Constants.Context.BienObligeNamespace}/status#{value}");
        IEnumerable<string> objectTypes = ["bienoblige:status", "Object"];
        return this.Status(statusUri, objectTypes);
    }

    public UpdateStatusActivityBuilder Status(Uri statusId, IEnumerable<string> objectTypes)
    {
        _statusBuilder = new ObjectBuilder()
            .ClearObjectTypes()
            .AddObjectTypes(objectTypes)
            .Id(statusId);
        return this;
    }

    public UpdateStatusActivityBuilder AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var kvp in additionalProperties)
            this.AddAdditionalProperty(kvp);
        return this;
    }

    public UpdateStatusActivityBuilder AddAdditionalProperty(string key, object value)
    {
        var kvp = new KeyValuePair<string, object>(key, value);
        return this.AddAdditionalProperty(kvp);
    }

    public UpdateStatusActivityBuilder AddAdditionalProperty(KeyValuePair<string, object> kvp)
    {
        _additionalProperties ??= new();
        _additionalProperties.Add(kvp.Key, kvp.Value);
        return this;
    }

    public UpdateStatusActivityBuilder ClearAdditionalProperties()
    {
        _additionalProperties = null;
        return this;
    }

    public UpdateStatusActivityBuilder Target(ObjectIdentifierBuilder value)
    {
        _targetBuilder = value;
        return this;
    }

}
