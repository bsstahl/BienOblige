using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BienOblige.ApiClient.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetHttpResponse(this HttpClient httpClient, ILogger logger, JsonContent payload)
    {
        // TODO: Add retry logic
        logger.LogInformation("Posting request to {Path}", Api.Constants.Path.ActivityInbox);
        var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, payload);
        logger.LogInformation("Response received: {Response}", JsonSerializer.Serialize(response));
        return response;
    }
}
