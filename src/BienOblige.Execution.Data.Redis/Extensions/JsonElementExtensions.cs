using System.Text.Json;

namespace BienOblige.Execution.Data.Redis.Extensions;

internal static class JsonElementExtensions
{
    internal static string GetStringProperty(this JsonElement element, string propertyName)
        => element.GetProperty(propertyName).GetString()!;

}
