using BienOblige.Execution.Application.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class Activity
{
    [JsonPropertyName("@context")]
    public Context? Context { get; set; }

    [JsonPropertyName("@type")]
    public required string @Type { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("bienoblige:correlationId")]
    public required string CorrelationId { get; set; }

    [JsonPropertyName("actor")]
    public required Actor Actor { get; set; }

    [JsonPropertyName("object")]
    public required NetworkObject Object { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }


    [JsonExtensionData]
    public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();

    public ActivityStream.Aggregates.Activity AsAggregate()
    {
        return new ActivityStream.Aggregates.Activity()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(this.Id),
            CorrelationId = ActivityStream.ValueObjects.NetworkIdentity.From(this.CorrelationId),
            ActivityType = this.Type.AsActivityType(),
            Actor = this.Actor.AsAggregate(),
            Object = this.Object.AsAggregate(),
            Published = this.Published ?? DateTimeOffset.UtcNow,
            ObjectTypeName = ActivityStream.Aggregates.Activity.GetObjectTypeName()
        };
    }

    public static Activity From(ActivityStream.Aggregates.Activity activity)
    {
        var id = activity.Id;
        var activityType = activity.ActivityType.ToString();
        var actor = Actor.From(activity.Actor);
        var correlationId = activity.CorrelationId.Value.ToString();

        return new Activity()
        {
            Id = id.Value.ToString(),
            CorrelationId = correlationId,
            Type = activityType,
            Actor = actor,
            Object = NetworkObject.From(activity.Object),
            Published = activity.Published ?? DateTimeOffset.UtcNow,
            Context = Context.From(activity.Context),
        };
    }

    public IEnumerable<ActivityStream.ValueObjects.TypeName> GetObjectTypeName() 
        => [ActivityStream.ValueObjects.TypeName.From(nameof(Activity))];
}
