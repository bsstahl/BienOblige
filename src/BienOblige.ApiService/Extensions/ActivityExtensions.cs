using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActivityExtensions
{
    public static ActivityStream.Aggregates.Activity AsAggregate(this Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);
        return new ActivityStream.Aggregates.Activity()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(activity.CorrelationId),
            ActivityType = Enum.Parse<ActivityStream.Enumerations.ActivityType>(activity.ActivityType.ToString()),
            Actor = activity.Actor.AsAggregate(),
            ActionItem = activity.ActionItem.AsAggregate(),
            Published = activity.Published ?? DateTimeOffset.UtcNow,
            ObjectTypeName = ActivityStream.Aggregates.Activity.GetObjectTypeName()
        };
    }
}
