using BienOblige.Api.Builders;
using BienOblige.Api.Entities;

namespace BienOblige.Api.Extensions;

public static class ObjectBuilderExtensions
{
    public static List<NetworkObject> Build(this IEnumerable<ObjectBuilder>? builders)
    {
        return (builders?.Select(b => b.Build()) ?? []).ToList();
    }
}
