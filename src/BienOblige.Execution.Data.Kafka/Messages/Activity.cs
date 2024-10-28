using BienOblige.Execution.Data.Kafka.Aggregates;
using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Activity
{
    private List<ValueObjects.Context> _context = new();
    private string _jsonMessage = string.Empty;

    public IEnumerable<ValueObjects.Context> Context
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

    public string ActivityType { get; private set; } = "Create";

    public string CorrelationId { get; set; }

    public Actor Actor { get; set; }

    public ActionItem ActionItem { get; set; }

    public DateTimeOffset Published { get; set; }

    override public string ToString() => _jsonMessage;
    public Activity(string jsonMessage, string correlationId)
    {
        _jsonMessage = jsonMessage;
        ArgumentNullException.ThrowIfNullOrWhiteSpace(jsonMessage, nameof(jsonMessage));

        this.CorrelationId = correlationId;

        using (JsonDocument doc = JsonDocument.Parse(jsonMessage))
        {
            var root = doc.RootElement;

            this.Context = root.GetProperty("@context")
                .EnumerateArray()
                .Select(x => new ValueObjects.Context(x));

            this.Published = DateTimeOffset.Parse(root.GetStringProperty(nameof(this.Published).ToLower()));
            this.Actor = new Actor(root.GetProperty(nameof(this.Actor).ToLower()));

            this.ActionItem = new ActionItem(root.GetProperty("object"));
        }
    }
}
