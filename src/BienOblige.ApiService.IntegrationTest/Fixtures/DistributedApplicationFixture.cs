using Aspire.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest.Fixtures;

public class DistributedApplicationFixture : IAsyncLifetime
{
    private bool _isConfigured = false;

    protected IDistributedApplicationTestingBuilder? AppHost { get; private set; }

    private DistributedApplication _app;
    public DistributedApplication App
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this.AppHost, nameof(this.AppHost));
            if (_app is null)
            {
                var appTask = this.AppHost.BuildAsync();
                appTask.Wait();
                _app = appTask.Result;

                var resourceNotificationService = _app.Services
                    .GetRequiredService<ResourceNotificationService>();

                var startTask = _app.StartAsync();
                startTask.Wait();

                var resourcesTask = resourceNotificationService
                    .WaitForResourceAsync("api", KnownResourceStates.Running)
                    .WaitAsync(TimeSpan.FromSeconds(60));
                resourcesTask.Wait();
            }

            return _app;
        }
    }

    public void Configure(ITestOutputHelper output)
    {
        if (!_isConfigured)
        {
            ArgumentNullException.ThrowIfNull(this.AppHost, nameof(this.AppHost));

            this.AppHost.Services.AddLogging(h =>
                h.AddXUnit(output)
                .SetMinimumLevel(LogLevel.Warning));

            _isConfigured = true;
        }
    }

    public async Task InitializeAsync()
    {
        this.AppHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.BienOblige_AppHost>();
        this.AppHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    }

    public async Task DisposeAsync()
    {
        if (_app is not null)
            await _app.DisposeAsync();
    }
}
