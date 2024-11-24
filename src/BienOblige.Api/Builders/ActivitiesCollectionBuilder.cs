using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActivitiesCollectionBuilder
{
    const string _defaultBaseUri = "https://bienoblige.com";

    private List<KeyValuePair<string?, string>> _context = Constants.Context.Default;

    private Uri? _correlationId;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ActionItemCollectionBuilder _actionItemCollectionBuilder = new();

    private readonly Uri _instanceBaseUri;

    public ActivitiesCollectionBuilder()
        : this(new Uri(_defaultBaseUri))
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

        // Assign Id's to all ActionItems
        _actionItemCollectionBuilder.AssignIds(_instanceBaseUri);

        var actionItems = new List<ActionItem>(_actionItemCollectionBuilder.Build());
        // actionItems.AddRange(_actionItems);
        if (!actionItems.Any())
            throw new ArgumentNullException($"At least 1 ActionItem must be supplied");

        // Define the published date anywhere it isn't set
        _published ??= DateTimeOffset.UtcNow;
        actionItems.ToList().ForEach(i => i.Published ??= _published);

        var updatingActor = _actorBuilder.Build();

        // Create an Activity for each ActionItem
        return actionItems.Select(x => new Activity()
        {
            Id = new Uri($"{_instanceBaseUri}Activity/{Guid.NewGuid()}"),
            Context = _context,
            CorrelationId = _correlationId,
            ActivityType = _activityType.Value.ToString(),
            Actor = updatingActor,
            ActionItem = x,
            Published = _published
        });
    }

    public ActivitiesCollectionBuilder ClearContext()
    {
        _context.Clear();
        return this;
    }

    public ActivitiesCollectionBuilder AddContext(string key, string value)
    {
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

    //public ActivitiesCollectionBuilder ActionItems(IEnumerable<ActionItem> value)
    //{
    //    _actionItems.AddRange(value);
    //    return this;
    //}
}
