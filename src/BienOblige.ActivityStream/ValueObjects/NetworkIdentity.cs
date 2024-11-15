using BienOblige.ActivityStream.Exceptions;
using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class NetworkIdentity : ValueOf<Uri, NetworkIdentity>
{
    public NetworkIdentity()
    { }

    public static NetworkIdentity From(string uri)
    {
        InvalidIdentifierException.ThrowIfInvalid(uri);
        return NetworkIdentity.From(new Uri(uri));
    }

    public static NetworkIdentity From(Guid guid)
    {
        return NetworkIdentity.From($"urn:uid:{guid}");
    }

    public static NetworkIdentity New()
    {
        return NetworkIdentity.From(Guid.NewGuid());
    }
}