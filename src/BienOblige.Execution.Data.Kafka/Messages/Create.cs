﻿using BienOblige.Execution.Data.Kafka.Aggregates;
using BienOblige.Execution.Data.Kafka.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Create
{
    private List<ValueObjects.Context> _context = new ();

    // TODO: Make the following properties optional
    // string targetType, string targetId, string targetName, string targetDescription

    public Create(string correlationId, DateTimeOffset published,
        string actionItemId, string actionItemName,
        string actorId, string actorType)
    {
        _context.Add(new ValueObjects.Context("https://www.w3.org/ns/activitystreams"));
        _context.Add(new ValueObjects.Context("https://bienoblige.com/ns", "bienoblige"));
        _context.Add(new ValueObjects.Context("https://schema.org", "schema"));

        this.CorrelationId = correlationId;
        this.Published = published;
        this.Actor = new Actor(actorId, actorType);

        this.ActionItem = new ActionItem(actionItemId, actionItemName);
    }

    [JsonConverter(typeof(ContextCollectionConverter))]
    [JsonPropertyName("@context")]
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

    [JsonPropertyName("@type")]
    public string ActivityType { get; private set; } = "Create";

    [JsonPropertyName("id")]
    public string CorrelationId { get; set; }

    [JsonPropertyName("actor")]
    public Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public ActionItem ActionItem { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    override public string ToString()
    {
        return JsonSerializer
            .Serialize(this, JsonSerializerOptions.Default.BienObligeDefaults());
    }
}

