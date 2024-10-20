using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class NetworkIdentityExtensions
{
    internal static NetworkIdentity CreateRandom(this NetworkIdentity? _)
        => NetworkIdentity.From($"https://example.org/{string.Empty.GetRandom()}");
}
