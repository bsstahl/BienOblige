using BienOblige.Api.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BienOblige.ApiClient;

public class Activities
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public Activities(ILogger logger, IConfiguration config, HttpClient httpClient)
    {
        _logger = logger;
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PublicationResult>> Publish(IEnumerable<Activity> activities)
    {
        return await Task.WhenAll(activities.Select(activity => this.Publish(activity)));
    }

    public async Task<PublicationResult> Publish(Activity activity)
    {
        // TODO: Add error handling
        var payload = JsonContent.Create(activity);
        var response = await _httpClient.PostAsync(Api.Constants.Path.Inbox, payload);
        if (response.IsSuccessStatusCode)
            return new PublicationResult(activity, true);
        else
        {
            var errors = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to publish activity: {@Activity}. Errors: {@Errors}", activity, errors);
            return new PublicationResult(activity, false, new[] { errors });
        }
    }
}
