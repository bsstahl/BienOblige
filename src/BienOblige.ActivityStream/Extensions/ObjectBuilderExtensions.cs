using BienOblige.ActivityStream.Builders;
using BienOblige.ActivityStream.Collections;

namespace BienOblige.ActivityStream.Extensions;

public static class ObjectBuilderExtensions
{
    public static NetworkObjectCollection? BuildCollection(this IEnumerable<ObjectBuilder>? builders)
    {
        NetworkObjectCollection? result = null;
        if (builders is not null)
        {
            result = NetworkObjectCollection.From(builders.Select(builder => builder.Build()));
        }
        return result;
    }
}
