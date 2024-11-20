using BienOblige.Execution.Application.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Activity
{
    [JsonPropertyName("@context")]
    public Context? Context { get; set; }

    [JsonPropertyName("@type")]
    public string @Type { get; private set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("actor")]
    public Actor? Actor { get; set; }

    [JsonPropertyName("object")]
    public ActionItem? ActionItem { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    //[JsonPropertyName("target")]
    //public ActionItem? Target { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtensionData { get; set; } = new();


    [JsonConstructor]
    public Activity(string id, string @type)
    {
        this.Id = id;
        this.@Type = @type;
    }

    //public Activity(string activityType, string correlationId, DateTimeOffset published,
    //    ActionItem actionItem, Context context, Actor updatingActor)
    //{
    //    this.ContextItems = context;
    //    this.ActivityType = activityType;
    //    this.CorrelationId = correlationId;
    //    this.Published = published;
    //    this.Actor = updatingActor;
    //    this.ActionItem = actionItem;
    //}

    //[JsonConstructor]
    //public Activity(string jsonMessage)
    //{
    //    ArgumentNullException.ThrowIfNullOrWhiteSpace(jsonMessage, nameof(jsonMessage));

    //    _jsonMessage = jsonMessage;

    //    using (JsonDocument doc = JsonDocument.Parse(jsonMessage))
    //    {
    //        var root = doc.RootElement;

    //        // TODO: Actually use the supplied context
    //        // var context = root.GetProperty("@context").ParseContext();
    //        //this.Context = new List<Context>()
    //        //{
    //        //    new Context(BienOblige.Constants.Namespaces.RootNamespaceName),
    //        //    new Context(BienOblige.Constants.Namespaces.BienObligeNamespaceName, BienOblige.Constants.Namespaces.BienObligeNamespaceKey),
    //        //    new Context(BienOblige.Constants.Namespaces.SchemaNamespaceName, BienOblige.Constants.Namespaces.SchemaNamespaceKey)
    //        //};

    //        this.CorrelationId = root.GetStringProperty("id");
    //        this.ActivityType = root.GetStringProperty("@type");
    //        this.Published = DateTimeOffset.Parse(root.GetStringProperty(nameof(this.Published).ToLower()));
    //        this.Actor = new Actor(root.GetProperty(nameof(this.Actor).ToLower()));

    //        this.ActionItem = new ActionItem(root.GetProperty("object"));
    //    }
    //}

    public ActivityStream.Aggregates.Activity AsAggregate()
    {
        return new ActivityStream.Aggregates.Activity(
            ActivityStream.ValueObjects.NetworkIdentity.From(this.Id),
            this.Type.AsActivityType(),
            this.Actor?.AsAggregate(),
            this.ActionItem?.AsAggregate(),
            this.Published ?? DateTimeOffset.UtcNow);
    }

    public static Activity From(ActivityStream.Aggregates.Activity activity)
    {
        var id = activity.Id.Value.ToString();
        var activityType = activity.ActivityType.ToString();
        return new Activity(id, activityType)
        {
            Published = activity.Published ?? DateTimeOffset.UtcNow,
            ActionItem = ActionItem.From(activity.ActionItem),
            Context = Context.From(activity.Context),
            Actor = Actor.From(activity.Actor)
        };
    }
}
