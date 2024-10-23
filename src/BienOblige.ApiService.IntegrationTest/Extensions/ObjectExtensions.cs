using Google.Protobuf.WellKnownTypes;

namespace BienOblige.ApiService.IntegrationTest.Extensions;

internal static class ObjectExtensions
{
    internal static DistributedApplication GetApp(this object _)
    {
        var appHostTask = DistributedApplicationTestingBuilder
            .CreateAsync<Projects.BienOblige_AppHost>();
        appHostTask.Wait();
        var appHost = appHostTask.Result;

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        var buildTask = appHost.BuildAsync();
        buildTask.Wait();

        var app = buildTask.Result;

        var resourceNotificationService = app.Services
            .GetRequiredService<ResourceNotificationService>();

        var startTask = app.StartAsync();
        startTask.Wait();

        var waitTask = resourceNotificationService.WaitForResourceAsync("api", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
        waitTask.Wait();

        return app;
    }
    
    internal static HttpClient GetApiClient(this DistributedApplication app)
        => app.CreateHttpClient("api");

}
