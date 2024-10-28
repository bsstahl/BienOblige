using Microsoft.Extensions.Logging;

namespace BienOblige.ApiService.IntegrationTest.Extensions;

internal static class DistributedApplicationExtensions
{
    internal static (ILogger, HttpClient) GetRequiredServices(
        this DistributedApplication? app, 
        Guid correlationId, 
        Guid userId)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resources = applicationModel.Resources;
        var resource = resources
            .SingleOrDefault(r => string.Equals(r.Name, "api", StringComparison.OrdinalIgnoreCase)) as IResourceWithEndpoints;

        var httpClient = app.CreateHttpClient("api");
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

        httpClient.DefaultRequestHeaders.Add("x-user-id", $"https://example.org/{userId}");
        httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId.ToString());

        var logger = app.Services.GetRequiredService<ILogger<Execution_Create_Should>>();
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        logger.LogInformation("Logger and HTTP Client created");

        return (logger, httpClient);
    }

}
