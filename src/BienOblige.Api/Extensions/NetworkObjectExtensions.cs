using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using System.Text.Json;

namespace BienOblige.Api.Extensions;

public static class NetworkObjectExtensions
{
    public static List<ObjectBuilder> AsObjectBuilders(this IEnumerable<NetworkObject>? networkObjects)
    {
        return (networkObjects?.Select(b => b.AsObjectBuilder()) ?? []).ToList();
    }

    public static ActionItem AsActionItem(this NetworkObject networkObject)
    {
        return networkObject.ObjectType.Contains("bienoblige:ActionItem")
            ? JsonSerializer.Deserialize<ActionItem>(JsonSerializer.Serialize(networkObject))
            : throw new InvalidOperationException("NetworkObject is not an ActionItem");
    }

}
