using BienOblige.ApiService.Entities;
using BienOblige.ApiService.Extensions;
using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.Execution.Builders;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BienOblige.ApiService.IntegrationTest;

public class Execution_Create_Should : IAsyncLifetime
{
    private DistributedApplication? _app;

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var (logger, httpClient) = _app.GetRequiredServices(Guid.NewGuid(), Guid.NewGuid());

        var content = new ActionItemBuilder()
            .UseRandomValues()
            .BuildJsonContent();
        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response Body: {@Response}", body);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task RespondWithTheSpecifiedCorrelationIdHeader()
    {
        var expectedCorrelationId = Guid.NewGuid();
        var (logger, httpClient) = _app.GetRequiredServices(expectedCorrelationId, Guid.NewGuid());

        var content = new ActionItemBuilder()
            .UseRandomValues()
            .BuildJsonContent();
        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        var actual = response.Headers.GetValues("X-Correlation-ID").Single();
        Assert.Equal(expectedCorrelationId.ToString(), actual);
    }

    [Fact]
    public async Task RespondWithTheSpecifiedActionItemId()
    {
        var (logger, httpClient) = _app.GetRequiredServices(Guid.NewGuid(), Guid.NewGuid());

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var content = actionItem.AsJsonContent();

        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        var actual = JsonSerializer.Deserialize<CreateResponse>(body);
        Assert.Equal(actionItem.Id.Value.ToString(), actual!.Id);
    }

    [Fact]
    public async Task ProduceAMessageOnTheActionItemStream()
    {
        var (logger, httpClient) = _app.GetRequiredServices(Guid.NewGuid(), Guid.NewGuid());

        var content = new ActionItemBuilder()
            .UseRandomValues()
            .BuildJsonContent();

        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        response.EnsureSuccessStatusCode();
    }

    #region Setup & Teardown

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.BienOblige_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        
        _app = await appHost.BuildAsync();

        var resourceNotificationService = _app.Services
            .GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();

        await resourceNotificationService.WaitForResourceAsync("api", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
    }

    public async Task DisposeAsync() => await _app!.DisposeAsync();

    #endregion
}