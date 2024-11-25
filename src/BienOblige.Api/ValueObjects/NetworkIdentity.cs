using ValueOf;

namespace BienOblige.Api.ValueObjects;

public class NetworkIdentity : ValueOf<Uri, NetworkIdentity>
{
    public NetworkIdentity()
    { }

    public static NetworkIdentity From(string uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out Uri? result)
            ? NetworkIdentity.From(result)
            : throw new ArgumentException($"Invalid Uri '{uri}'");
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