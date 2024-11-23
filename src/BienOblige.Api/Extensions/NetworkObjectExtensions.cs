using BienOblige.Api.Builders;
using BienOblige.Api.Entities;

namespace BienOblige.Api.Extensions;

public static class NetworkObjectExtensions
{
    public static List<ObjectBuilder> AsObjectBuilders(this IEnumerable<NetworkObject>? networkObjects)
    {
        return (networkObjects?.Select(b => b.AsObjectBuilder()) ?? []).ToList();
    }
}
