using BienOblige.Execution.Builders;
using BienOblige.ActivityStream.Constants;
using BienOblige.ActivityStream.Enumerations;
using BienOblige.Execution.Application.Extensions;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Data.Kafka.Extensions;
using BienOblige.ActivityStream.Builders;

namespace BienOblige.Execution.Data.Kafka.Builders;

public class ActivityMessageBuilder
{
    // Required Nodes
    private string? _correlationId;
    private DateTimeOffset? _published;
    private ActivityType? _activityType;

    //private string? _actionItemId;
    //private string? _actionItemName;
    //private string? _actionItemContent;
    private ActionItemBuilder? _actionItemBuilder;

    //private string? _actorId;
    //private string? _actorType;
    private ActorBuilder? _actorBuilder;

    // Optional Nodes
    private string? _targetType;
    private string? _targetId;
    private string? _targetName;
    private string? _targetDescription;

    public Messages.Activity Build()
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_correlationId);
        ArgumentNullException.ThrowIfNull(_published);
        ArgumentNullException.ThrowIfNull(_activityType);

        ArgumentNullException.ThrowIfNull(_actionItemBuilder);
        ArgumentNullException.ThrowIfNull(_actorBuilder);

        var actionItem = _actionItemBuilder.Build();
        var actor = _actorBuilder.Build();
        var context = new List<Messages.Context>()
            {
                new Messages.Context(Namespaces.RootNamespaceName),
                new Messages.Context(Namespaces.BienObligeNamespaceName, Namespaces.BienObligeNamespaceKey),
                new Messages.Context(Namespaces.SchemaNamespaceName, Namespaces.SchemaNamespaceKey)
            };

        var result = new Messages.Activity(
            activityType: _activityType.Value.ToString(),
            correlationId: _correlationId,
            published: _published.Value,
            context: context,
            actionItem: actionItem,
            updatingActor: actor);

        // TODO: Add optional fields if supplied
        
        // TODO: Add ActionItemContent
        // TODO: Add ActionItemSummary


        return result;
    }

    public ActivityMessageBuilder ActivityType(string activityType)
    {
        return this.ActivityType(activityType.AsActivityType());
    }

    public ActivityMessageBuilder ActivityType(ActivityType value)
    {
        _activityType = value;
        return this;
    }

    public ActivityMessageBuilder CorrelationId(Guid correlationId)
    {
        return this.CorrelationId(correlationId.AsNetworkIdentity());
    }

    public ActivityMessageBuilder CorrelationId(NetworkIdentity correlationId)
    {
        return this.CorrelationId(correlationId.ToString());
    }

    public ActivityMessageBuilder CorrelationId(string correlationId)
    {
        _correlationId = correlationId;
        return this;
    }

    public ActivityMessageBuilder PublishedNow()
    {
        return this.Published(DateTimeOffset.UtcNow);
    }

    public ActivityMessageBuilder Published(DateTimeOffset published)
    {
        _published = published;
        return this;
    }

    public ActivityMessageBuilder ActionItem(ActionItemBuilder builder)
    {
        _actionItemBuilder = builder;
        return this;
    }

    public ActivityMessageBuilder Actor(ActorBuilder builder)
    {
        _actorBuilder = builder;
        return this;
    }
}
