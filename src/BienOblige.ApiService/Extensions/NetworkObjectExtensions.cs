using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class NetworkObjectExtensions
{
    public static ActivityStream.Aggregates.NetworkObject AsAggregate(this NetworkObject value)
    {
        ArgumentNullException.ThrowIfNull(value.ObjectId, nameof(value.ObjectId));
        return new ActivityStream.Aggregates.NetworkObject()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(value.ObjectId),
            ObjectTypeName = value.ObjectType.Select(t => ActivityStream.ValueObjects.TypeName.From(t))
        };
    }

}
