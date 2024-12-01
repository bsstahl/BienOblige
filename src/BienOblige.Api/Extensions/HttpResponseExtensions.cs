using System.Text.Json;

namespace BienOblige.Api.Extensions;

public static class HttpResponseExtensions
{
   public static async Task<IEnumerable<string>> GetPublicationFailures(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var responseError = $"Response code {((int)response.StatusCode)} ({response.StatusCode}) does not indicate success. Content: {content}";
        return [responseError];
    }
}
