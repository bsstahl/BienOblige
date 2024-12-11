using BienOblige.Api.Entities;

namespace BienOblige.ApiService.Extensions;

public static class NetworkObjectExtensions
{
    public static ActivityStream.Aggregates.NetworkObject AsAggregate(this NetworkObject value)
    {
        ArgumentNullException.ThrowIfNull(value.Id, nameof(value.Id));
        return new ActivityStream.Aggregates.NetworkObject()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(value.Id),
            ObjectTypeName = value.ObjectType.Select(t => ActivityStream.ValueObjects.TypeName.From(t))
        };
    }

}
