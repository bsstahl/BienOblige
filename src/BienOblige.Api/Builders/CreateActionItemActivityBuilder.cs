using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class CreateActionItemActivityBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ActionItemBuilder? _actionItemBuilder;
    private Dictionary<string, object>? _additionalProperties;

    private readonly Uri _instanceBaseUri;

    public CreateActionItemActivityBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public CreateActionItemActivityBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public Activity Build()
    {
        ArgumentNullException.ThrowIfNull(_actionItemBuilder, nameof(_actionItemBuilder));

        // Assign default values where needed
        _activityType = ActivityType.Create;
        _actionItemBuilder.AssignId(_instanceBaseUri);
        _actionItemBuilder.Published(_published, overwrite: false);

        var actionItem = _actionItemBuilder.Build(_activityType.Value).Single();
        return this.Build(actionItem);
    }

    private Activity Build(ActionItem actionItem)
    {
        ArgumentNullException.ThrowIfNull(_activityType, nameof(_activityType));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));

        // Assign default values where needed
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
            Object = actionItem.AsNetworkObject(),
            Published = _published,
            AdditionalProperties = _additionalProperties ?? new()
        };
    }

    public CreateActionItemActivityBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public CreateActionItemActivityBuilder AddContext(string key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public CreateActionItemActivityBuilder AddContext(IEnumerable<KeyValuePair<string?, string>> context)
    {
        _context ??= new();
        _context.AddRange(context);
        return this;
    }

    public CreateActionItemActivityBuilder CorrelationId(Guid value)
    {
        return this.CorrelationId($"urn:uid:{value.ToString()}");
    }

    public CreateActionItemActivityBuilder CorrelationId(string value)
    {
        return this.CorrelationId(new Uri(value));
    }

    public CreateActionItemActivityBuilder CorrelationId(Uri value)
    {
        _correlationId = value;
        return this;
    }

    public CreateActionItemActivityBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public CreateActionItemActivityBuilder Published(DateTimeOffset? value)
    {
        _published = value;
        return this;
    }

    public CreateActionItemActivityBuilder ActionItem(ActionItemBuilder value)
    {
        _actionItemBuilder = value;
        return this;
    }

    public CreateActionItemActivityBuilder AssignToLocation(NetworkIdentity actionItemId, LocationBuilder locationBuilder)
    {
        return this.AssignToLocation(actionItemId, locationBuilder.Build());
    }

    public CreateActionItemActivityBuilder AssignToLocation(NetworkIdentity actionItemId, Place location)
    {
        return this.AssignToLocation(actionItemId.Value, location.AsObjectBuilder());
    }

    public CreateActionItemActivityBuilder AssignToLocation(Uri actionItemId, ObjectBuilder locationBuilder)
    {
        _actionItemBuilder = new ActionItemBuilder()
            .Id(actionItemId)
            .Location(locationBuilder);
        return this;
    }

    public CreateActionItemActivityBuilder AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var kvp in additionalProperties)
            this.AddAdditionalProperty(kvp);
        return this;
    }

    public CreateActionItemActivityBuilder AddAdditionalProperty(string key, object value)
    {
        var kvp = new KeyValuePair<string, object>(key, value);
        return this.AddAdditionalProperty(kvp);
    }

    public CreateActionItemActivityBuilder AddAdditionalProperty(KeyValuePair<string, object> kvp)
    {
        _additionalProperties ??= new();
        _additionalProperties.Add(kvp.Key, kvp.Value);
        return this;
    }

    public CreateActionItemActivityBuilder ClearAdditionalProperties()
    {
        _additionalProperties = null;
        return this;
    }
}
