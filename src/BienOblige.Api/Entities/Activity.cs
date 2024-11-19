using BienOblige.Api.Enumerations;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Activity
{
    [JsonPropertyName("id")]
    public Uri CorrelationId { get; set; }

    [JsonPropertyName("@type")]
    public string ActivityType { get; set; }

    [JsonPropertyName("actor")]
    public Actor Actor { get; set; }

    [JsonPropertyName("target")]
    public ActionItem Target { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }


    public Activity()
    { }

    public Activity(Uri correlationId, ActivityType activityType, Actor updatingActor,
        ActionItem actionItem, DateTimeOffset? published = null)
    {
        this.CorrelationId = correlationId;
        this.ActivityType = activityType.ToString();
        this.Actor = updatingActor;
        this.Target = actionItem;
        this.Published = published ?? DateTimeOffset.UtcNow;
    }
}
