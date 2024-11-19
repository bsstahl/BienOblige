using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class NetworkObjectExtensions
{
    public static ActivityStream.Aggregates.NetworkObject AsAggregate(this NetworkObject value)
    {
        ArgumentNullException.ThrowIfNull(value.ObjectId, nameof(value.ObjectId));
        return new ActivityStream.Aggregates.NetworkObject(
            ActivityStream.ValueObjects.NetworkIdentity.From(value.ObjectId),
            ActivityStream.ValueObjects.TypeName.From(value.ObjectType));
    }

}
