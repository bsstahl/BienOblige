using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActivityExtensions
{
    public static ActivityStream.Aggregates.Activity AsAggregate(this Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);
        return new ActivityStream.Aggregates.Activity(
            ActivityStream.ValueObjects.NetworkIdentity.From(activity.CorrelationId),
            Enum.Parse<ActivityStream.Enumerations.ActivityType>(activity.ActivityType.ToString()),
            activity.Actor.AsAggregate(),
            activity.Target.AsAggregate(),
            activity.Published ?? DateTimeOffset.UtcNow
        );
    }
}
