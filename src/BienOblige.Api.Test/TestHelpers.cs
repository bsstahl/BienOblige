using BienOblige.Api.Entities;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public static class TestHelpers
{
    public static (string City, string State)[] CityPairs = [
        ("Phoenix", "AZ"),
        ("Los Angeles", "CA"),
        ("Denver", "CO"),
        ("Miami", "FL"),
        ("Chicago", "IL"),
        ("New Orleans", "LA"),
        ("Boston", "MA"),
        ("Detroit", "MI"),
        ("Minneapolis", "MN"),
        ("Kansas City", "MO"),
        ("Las Vegas", "NV"),
        ("New York", "NY"),
        ("Portland", "OR"),
        ("Philadelphia", "PA"),
        ("Nashville", "TN"),
        ("Dallas", "TX"),
        ("Seattle", "WA")];

    public static string[] ResoCodes = new[] { "RESI", "RLSE", "RINC", "LAND", "MOBI", "FARM", "COMS", "COML", "BUSO" };

    public static string[] TagObjectTypes = new[] {"Article", "Audio", "Document", "Image", "Video", "Note" };

    public static void Log(this NetworkObject value, ILogger logger)
    {
        logger.LogTrace("NetworkObject: {@NetworkObject}", JsonSerializer.Serialize(value));
    }
}
