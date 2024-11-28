using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class ActivityBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ActionItemBuilder? _actionItemBuilder;

    private readonly Uri _instanceBaseUri;

    public ActivityBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public ActivityBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public Activity Build()
    {
        ArgumentNullException.ThrowIfNull(_actionItemBuilder, nameof(_actionItemBuilder));
        ArgumentNullException.ThrowIfNull(_activityType, nameof(_activityType));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));

        // Assign default values where needed
        _context ??= Constants.Context.Default;
        _correlationId ??= new Uri($"{_instanceBaseUri}/Activity/{Guid.NewGuid()}");
        _actionItemBuilder.AssignId(_instanceBaseUri);
        _published ??= DateTimeOffset.UtcNow;
        _actionItemBuilder.Published(_published, overwrite: false);

        var activity = new Activity()
        {
            Id = NetworkIdentity.From(_instanceBaseUri.ToString(), 
                nameof(Activity), Guid.NewGuid().ToString()).Value,
            Context = _context,
            CorrelationId = _correlationId,
            ActivityType = _activityType.Value.ToString(),
            Actor = _actorBuilder.Build(),
            ActionItem = _actionItemBuilder.Build(_activityType.Value).Single(),
            Published = _published
        };

        return activity;
    }

    public ActivityBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public ActivityBuilder AddContext(string key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public ActivityBuilder AddContext(IEnumerable<KeyValuePair<string?, string>> context)
    {
        _context ??= new();
        _context.AddRange(context);
        return this;
    }

    public ActivityBuilder CorrelationId(Guid value)
    {
        return this.CorrelationId($"urn:uid:{value.ToString()}");
    }

    public ActivityBuilder CorrelationId(string value)
    {
        return this.CorrelationId(new Uri(value));
    }

    public ActivityBuilder CorrelationId(Uri value)
    {
        _correlationId = value;
        return this;
    }

    public ActivityBuilder ActivityType(ActivityType? value)
    {
        _activityType = value;
        return this;
    }

    public ActivityBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public ActivityBuilder Published(DateTimeOffset? value)
    {
        _published = value;
        return this;
    }

    public ActivityBuilder ActionItem(ActionItemBuilder value)
    {
        _actionItemBuilder = value;
        return this;
    }

    public ActivityBuilder AssignToLocation(NetworkIdentity actionItemId, LocationBuilder locationBuilder)
    {
        return this.AssignToLocation(actionItemId, locationBuilder.Build());
    }

    public ActivityBuilder AssignToLocation(NetworkIdentity actionItemId, Location location)
    {
        return this.AssignToLocation(actionItemId.Value, location.AsObjectBuilder());
    }

    public ActivityBuilder AssignToLocation(Uri actionItemId, ObjectBuilder locationBuilder)
    {
        _actionItemBuilder = new ActionItemBuilder()
            .Id(actionItemId)
            .Location(locationBuilder);
        return this;
    }
}
