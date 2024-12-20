﻿using BienOblige.Api.Entities;
using BienOblige.Api.Extensions;
using BienOblige.ApiClient.Extensions;
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
        using (_logger.BeginScope(new Dictionary<string, object> 
        { 
            { "Method", "BienOblige.ApiClient.Activities.Publish" },
            { "MethodOverload", "IEnumerable<Activity>" }
        }))
        {
            var payload = JsonContent.Create(activities);

            IEnumerable<PublicationResult> results;
            try
            {
                var result = await _httpClient.GetHttpResponse(_logger, payload);
                results = await GetResponseDetails(result, activities);
            }
            catch (Exception ex)
            {
                var errors = ex.GetExceptionMessages();
                results = activities.Select(a => new PublicationResult(a, errors));
                _logger.LogError(ex, "Error publishing activities: {@Errors}", errors);
            }

            _logger.LogTrace("Results: {@Results}", results);

            return results;
        }
    }

    public async Task<PublicationResult> Publish(Activity activity)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "BienOblige.ApiClient.Activities.Publish" },
            { "MethodOverload", "Activity" }
        }))
        {
            var payload = JsonContent.Create(activity);

            IEnumerable<PublicationResult> results;
            try
            {
                var result = await _httpClient.GetHttpResponse(_logger, payload);
                results = await GetResponseDetails(result, [activity]);
            }
            catch (Exception ex)
            {
                var errors = ex.GetExceptionMessages();
                results = [new PublicationResult(activity, errors)];
                _logger.LogError(ex, "Error publishing activities: {@Errors}", errors);
            }

            _logger.LogTrace("Results: {@Results}", results);

            return results.Single();
        }
    }

    private async Task<IEnumerable<PublicationResult>> GetResponseDetails(HttpResponseMessage response, IEnumerable<Activity> requestActivities)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "GetResponseDetails" }
        }))
        {
            IEnumerable<PublicationResult> results;
            if (response.IsSuccessStatusCode)
            {
                var resultText = await response.Content.ReadAsStringAsync();
                _logger.LogTrace("Response: {Response}", resultText);
                results = await response.Content.ReadFromJsonAsync<IEnumerable<PublicationResult>>() ?? Array.Empty<PublicationResult>();
            }
            else
            {
                try
                {
                    results = await response.Content.ReadFromJsonAsync<IEnumerable<PublicationResult>>() ?? Array.Empty<PublicationResult>();
                }
                catch (Exception)
                {
                    var errors = await response.GetPublicationFailures();
                    _logger.LogError("{@Errors} - Activity: {@Activity}", errors, requestActivities);
                    results = requestActivities.Select(a => new PublicationResult(a, errors));
                }
            }

            return results;
        }    
    }
}
