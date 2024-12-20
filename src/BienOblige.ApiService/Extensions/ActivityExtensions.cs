﻿using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class ActivityExtensions
{
    public static ActivityStream.Aggregates.Activity AsAggregate(this Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity, nameof(activity));
        ArgumentNullException.ThrowIfNull(activity.CorrelationId, nameof(activity.CorrelationId));

        return new ActivityStream.Aggregates.Activity()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(activity.Id),
            CorrelationId = ActivityStream.ValueObjects.NetworkIdentity.From(activity.CorrelationId),
            ActivityType = Enum.Parse<ActivityStream.Enumerations.ActivityType>(activity.ActivityType.ToString()),
            Actor = activity.Actor.AsAggregate(),
            Object = activity.Object.AsAggregate(),
            Published = activity.Published ?? DateTimeOffset.UtcNow,
            ObjectTypeName = ActivityStream.Aggregates.Activity.GetObjectTypeName()
        };
    }
}
