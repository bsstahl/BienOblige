using BienOblige.Exceptions;
using ValueOf;

namespace BienOblige.ValueObjects;

public class NetworkIdentity : ValueOf<Uri, NetworkIdentity>
{
    public static NetworkIdentity From(string uri)
    {
        InvalidIdentifierException.ThrowIfInvalid(uri);
        return NetworkIdentity.From(new Uri(uri));
    }
}
