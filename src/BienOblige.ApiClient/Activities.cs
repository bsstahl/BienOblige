using BienOblige.Api.Entities;
using BienOblige.Api.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

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
        var payload = JsonContent.Create(activity);
        var result = new PublicationResult(activity, true);

        try
        {
            var response = await _httpClient.PostAsync(Api.Constants.Path.ActivityInbox, payload);
            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.GetPublicationFailures();
                _logger.LogError("{@Errors}\r\n\r\nActivity: {@Activity}", errors, activity);
                result = new PublicationResult(activity, false, errors);
            }
        }
        catch (Exception ex)
        {
            result = new PublicationResult(activity, false, ex.GetExceptionMessages());
        }

        return result;
    }

}
