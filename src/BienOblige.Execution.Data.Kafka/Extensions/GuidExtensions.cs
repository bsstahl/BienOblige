using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Data.Kafka.Extensions;

public static class GuidExtensions
{
    public static NetworkIdentity AsNetworkIdentity(this Guid guid)
    {
        return NetworkIdentity.From($"urn:uid:{guid.ToString()}");
    }
}
