using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActivitiesCollectionBuilder
{
    private Uri? _id;
    private ActivityType? _activityType;
    private ActorBuilder? _actorBuilder;
    private DateTimeOffset? _published;

    private ActionItemCollectionBuilder _actionItemCollectionBuilder = new();
    private List<ActionItem> _actionItems = new();

    public IEnumerable<Activity> Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_activityType, nameof(_activityType));
        ArgumentNullException.ThrowIfNull(_actorBuilder, nameof(_actorBuilder));

        var actionItems = new List<ActionItem>(_actionItems);
        actionItems.AddRange(_actionItemCollectionBuilder.Build());
        if (!actionItems.Any())
            throw new ArgumentNullException($"At least 1 ActionItem must be supplied");

        var published = _published ?? DateTimeOffset.UtcNow;
        var updatingActor = _actorBuilder.Build();

        return actionItems.Select(x => new Activity(_id, _activityType.Value, updatingActor, x, _published));
    }

    public ActivitiesCollectionBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ActivitiesCollectionBuilder Id(string value)
    {
        return this.Id(new Uri(value));
    }

    public ActivitiesCollectionBuilder Id(Uri value)
    {
        _id = value;
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

    public ActivitiesCollectionBuilder ActionItems(IEnumerable<ActionItem> value)
    {
        _actionItems.AddRange(value);
        return this;
    }
}
