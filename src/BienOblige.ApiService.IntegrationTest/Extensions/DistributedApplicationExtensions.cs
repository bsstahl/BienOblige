using Aspire.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BienOblige.ApiService.IntegrationTest.Extensions;

internal static class DistributedApplicationExtensions
{
    internal static (ILogger<T>, IConfiguration, HttpClient) GetRequiredServices<T>(
        this DistributedApplication? app, 
        Guid correlationId, 
        Guid actorId, string actorType)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        var apiResourceName = BienOblige.Constants.ServiceNames.ApiService;
        var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resources = applicationModel.Resources;
        var resource = resources
            .SingleOrDefault(r => string.Equals(r.Name, apiResourceName, StringComparison.OrdinalIgnoreCase)) as IResourceWithEndpoints;

        var httpClient = app.CreateHttpClient(apiResourceName);
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

        var config = app.Services.GetRequiredService<IConfiguration>();

        httpClient.DefaultRequestHeaders.Add("x-updatedby-id", $"https://example.org/{actorId}");
        httpClient.DefaultRequestHeaders.Add("x-updatedby-type", actorType);
        httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId.ToString());

        var logger = app.Services.GetRequiredService<ILogger<T>>();

        logger.LogInformation("Logger and HTTP Client created");

        return (logger, config, httpClient);
    }

}
