using BienOblige.Execution.Builders;

namespace BienOblige.Execution.Data.Kafka.Builders;

public class CreateMessageBuilder
{
    // Required Nodes
    private string? _correlationId;
    private DateTimeOffset? _published;

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

    public Messages.Create Build()
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_correlationId);
        ArgumentNullException.ThrowIfNull(_published);

        ArgumentNullException.ThrowIfNull(_actionItemBuilder);
        ArgumentNullException.ThrowIfNull(_actorBuilder);

        var actionItem = _actionItemBuilder.Build();
        var actor = _actorBuilder.Build();

        var result = new Messages.Create(
            correlationId: _correlationId,
            published: _published.Value,
            actionItemId: actionItem.Id.Value.ToString(),
            actionItemName: actionItem.Title.Value,
            actionItemContent: actionItem.Content.Value,
            actorId: actor.Id.Value.ToString(),
            actorType: actor.Type.ToString());

        // TODO: Add optional fields if supplied
        
        // TODO: Add ActionItemContent
        // TODO: Add ActionItemSummary


        return result;
    }

    public CreateMessageBuilder CorrelationId(string correlationId)
    {
        _correlationId = correlationId;
        return this;
    }

    public CreateMessageBuilder PublishedNow()
    {
        return this.Published(DateTimeOffset.UtcNow);
    }

    public CreateMessageBuilder Published(DateTimeOffset published)
    {
        _published = published;
        return this;
    }

    public CreateMessageBuilder ActionItem(ActionItemBuilder builder)
    {
        _actionItemBuilder = builder;
        return this;
    }

    public CreateMessageBuilder Actor(ActorBuilder builder)
    {
        _actorBuilder = builder;
        return this;
    }
}
