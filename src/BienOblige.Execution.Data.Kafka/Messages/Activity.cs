using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Activity
{
    private List<ContextItem> _context = new();
    private string _jsonMessage = string.Empty;

    [JsonPropertyName("@context")]
    public IEnumerable<ContextItem> ContextItems
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value.ToList();
        }
    }

    [JsonPropertyName("@type")]
    public string ActivityType { get; private set; }

    [JsonPropertyName("id")]
    public string CorrelationId { get; set; }

    [JsonPropertyName("actor")]
    public Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public ActionItem ActionItem { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    [JsonPropertyName("target")]
    public ActionItem Target { get; set; }


    override public string ToString() => _jsonMessage;


    public Activity(string activityType, string correlationId, DateTimeOffset published,
        ActionItem actionItem, Context context, Actor updatingActor)
    {
        this.ContextItems = context;
        this.ActivityType = activityType;
        this.CorrelationId = correlationId;
        this.Published = published;
        this.Actor = updatingActor;
        this.ActionItem = actionItem;

        _jsonMessage = JsonSerializer.Serialize(this);
    }

    [JsonConstructor]
    public Activity(string jsonMessage)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(jsonMessage, nameof(jsonMessage));

        _jsonMessage = jsonMessage;

        using (JsonDocument doc = JsonDocument.Parse(jsonMessage))
        {
            var root = doc.RootElement;

            // TODO: Actually use the supplied context
            // var context = root.GetProperty("@context").ParseContext();
            //this.Context = new List<Context>()
            //{
            //    new Context(BienOblige.Constants.Namespaces.RootNamespaceName),
            //    new Context(BienOblige.Constants.Namespaces.BienObligeNamespaceName, BienOblige.Constants.Namespaces.BienObligeNamespaceKey),
            //    new Context(BienOblige.Constants.Namespaces.SchemaNamespaceName, BienOblige.Constants.Namespaces.SchemaNamespaceKey)
            //};

            this.CorrelationId = root.GetStringProperty("id");
            this.ActivityType = root.GetStringProperty("@type");
            this.Published = DateTimeOffset.Parse(root.GetStringProperty(nameof(this.Published).ToLower()));
            this.Actor = new Actor(root.GetProperty(nameof(this.Actor).ToLower()));

            this.ActionItem = new ActionItem(root.GetProperty("object"));
        }
    }

    public ActivityStream.Aggregates.Activity AsAggregate()
    {
        return new ActivityStream.Aggregates.Activity(
            ActivityStream.ValueObjects.NetworkIdentity.From(this.CorrelationId),
            this.ActivityType.AsActivityType(),
            this.Actor.AsAggregate(),
            this.ActionItem.AsAggregate(),
            this.Published);
    }

    public static Activity From(ActivityStream.Aggregates.Activity activity)
    {
        return new Activity(
            activity.ActivityType.ToString(),
            activity.Id.Value.ToString(),
            activity.Published ?? DateTimeOffset.UtcNow,
            ActionItem.From(activity.Target),
            Context.From(activity.Context),
            Actor.From(activity.Actor))
        { };
    }
}
