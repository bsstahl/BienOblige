using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActivityBuilder
{
    private List<KeyValuePair<string?, string>> _context = Constants.Context.Default;

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
        ArgumentNullException.ThrowIfNull(_activityType, nameof(_activityType));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));

        _correlationId ??= new Uri($"{_instanceBaseUri}/Activity/{Guid.NewGuid()}");

        // Assign an Id to the ActionItem if needed
        _actionItemBuilder?.AssignId(_instanceBaseUri);

        var actionItem = _actionItemBuilder?.Build().Single();
        if (actionItem is null)
            throw new ArgumentNullException($"An ActionItem must be supplied");

        // Define the published date anywhere it isn't set
        _published ??= DateTimeOffset.UtcNow;
        actionItem.Published ??= _published;

        var updatingActor = _actorBuilder.Build();

        // Create an Activity for each ActionItem
        return new Activity()
        {
            Id = new Uri($"{_instanceBaseUri}Activity/{Guid.NewGuid()}"),
            Context = _context,
            CorrelationId = _correlationId,
            ActivityType = _activityType.Value.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem,
            Published = _published
        };
    }

    public ActivityBuilder ClearContext()
    {
        _context.Clear();
        return this;
    }

    public ActivityBuilder AddContext(string key, string value)
    {
        _context.Add(new KeyValuePair<string?, string>(key, value));
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

    public ActivityBuilder ActivityType(ActivityType value)
    {
        _activityType = value;
        return this;
    }

    public ActivityBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

    public ActivityBuilder Published(DateTimeOffset value)
    {
        _published = value;
        return this;
    }

    public ActivityBuilder ActionItem(ActionItemBuilder value)
    {
        _actionItemBuilder = value;
        return this;
    }

}
