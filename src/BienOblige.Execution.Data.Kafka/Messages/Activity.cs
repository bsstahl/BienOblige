using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Application.Extensions;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Activity
{
    // private List<Context> _context = new();
    private string _jsonMessage = string.Empty;

    //[JsonPropertyName("@context")]
    //public IEnumerable<Context> Context
    //{
    //    get
    //    {
    //        return _context;
    //    }
    //    set
    //    {
    //        _context = value.ToList();
    //    }
    //}

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


    override public string ToString() => _jsonMessage;


    public Activity(string activityType, string correlationId, DateTimeOffset published,
        Aggregates.ActionItem actionItem, IEnumerable<Context> context, BienOblige.ActivityStream.Aggregates.Actor updatingActor)
    {
        // _context.Add(new Context(BienOblige.Constants.Namespaces.RootNamespaceName));
        // context.Where(c => c.HasKey).ToList().ForEach(c => _context.Add(c));

        this.ActivityType = activityType;
        this.CorrelationId = correlationId;
        this.Published = published;
        this.Actor = Actor.From(updatingActor);

        this.ActionItem = ActionItem.From(actionItem);

        _jsonMessage = JsonSerializer.Serialize(this);
    }

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

    public Application.Aggregates.Activity AsAggregate()
    {
        return new Application.Aggregates.Activity(
            NetworkIdentity.From(this.CorrelationId),
            this.ActivityType.AsActivityType(),
            this.Actor.AsAggregate(),
            this.ActionItem.AsAggregate(),
            this.Published);
    }
}
