using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActivitiesCollectionBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ActionItemCollectionBuilder _actionItemCollectionBuilder = new();
    private Dictionary<string, object>? _additionalProperties;

    private readonly Uri _instanceBaseUri;


    public ActivitiesCollectionBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public ActivitiesCollectionBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public IEnumerable<Activity> Build()
    {
        ArgumentNullException.ThrowIfNull(_activityType, nameof(_activityType));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));

        _correlationId ??= new Uri($"{_instanceBaseUri}/Activity/{Guid.NewGuid()}");
        _context ??= Constants.Context.Default;

        // Assign Id's to all ActionItems
        _actionItemCollectionBuilder.AssignIds(_instanceBaseUri);

        if (!_actionItemCollectionBuilder.Any())
            throw new ArgumentNullException($"At least 1 ActionItem must be supplied");

        // Define the published date anywhere it isn't set
        _published ??= DateTimeOffset.UtcNow;
        _actionItemCollectionBuilder.Published(_published, overwrite: false);

        var actionItemBuilders = _actionItemCollectionBuilder.GetAllBuilders();

        // Create an Activity for each ActionItem
        return actionItemBuilders.Select(x => new ActivityBuilder()
            .AddContext(_context)
            .AddAdditionalProperties(_additionalProperties ?? [])
            .CorrelationId(_correlationId)
            .ActivityType(_activityType)
            .Actor(_actorBuilder)
            .ActionItem(x)
            .Published(_published)
            .Build())
        .ToList();
    }

    public ActivitiesCollectionBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public ActivitiesCollectionBuilder AddContext(string key, string value)
    {
        _context ??= new();
        _context.Add(new KeyValuePair<string?, string>(key, value));
        return this;
    }

    public ActivitiesCollectionBuilder CorrelationId(Guid value)
    {
        return this.CorrelationId($"urn:uid:{value.ToString()}");
    }

    public ActivitiesCollectionBuilder CorrelationId(string value)
    {
        return this.CorrelationId(new Uri(value));
    }

    public ActivitiesCollectionBuilder CorrelationId(Uri value)
    {
        _correlationId = value;
        return this;
    }

    public ActivitiesCollectionBuilder ActivityType(ActivityType value)
    {
        _activityType = value;
        return this;
    }

    public ActivitiesCollectionBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public ActivitiesCollectionBuilder Published(DateTimeOffset value)
    {
        _published = value;
        return this;
    }

    public ActivitiesCollectionBuilder ActionItems(ActionItemCollectionBuilder value)
    {
        _actionItemCollectionBuilder = value;
        return this;
    }

    public ActivitiesCollectionBuilder AddAdditionalProperty(string key, object value)
    {
        _additionalProperties ??= new();
        _additionalProperties.Add(key, value);
        return this;
    }

    public ActivitiesCollectionBuilder ClearAdditionalProperties()
    {
        _additionalProperties = null;
        return this;
    }
}
