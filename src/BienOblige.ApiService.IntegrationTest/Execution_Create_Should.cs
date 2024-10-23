using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.Execution.Builders;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest;

public class Execution_Create_Should : IDisposable
{
    private bool _disposedValue;

    private readonly DistributedApplication _app;
    private readonly ITestOutputHelper _output;

    public Execution_Create_Should(ITestOutputHelper output)
    {
        _output = output;
        _app = this.GetApp();
    }

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var httpClient = _app.GetApiClient();

        httpClient.DefaultRequestHeaders.Add("x-user-id", "https://example.org/4719a2cc-1d81-43b9-a91b-bfdadc0c8765");
        httpClient.DefaultRequestHeaders.Add("x-correlation-id", "1285a46a-1b8d-490c-9dee-bc88fd1494a4");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var content = JsonContent.Create(actionItem);

        var response = await httpClient.PostAsync("/api/Execution/", content);
        
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task ProduceAMessageOnTheActionItemStream()
    {
        var logger = _app.Services.GetRequiredService<ILogger<Execution_Create_Should>>();
        var httpClient = _app.GetApiClient();

        httpClient.DefaultRequestHeaders.Add("x-user-id", "https://example.org/4719a2cc-1d81-43b9-a91b-bfdadc0c8765");
        httpClient.DefaultRequestHeaders.Add("x-correlation-id", "1285a46a-1b8d-490c-9dee-bc88fd1494a4");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Build();

        var content = JsonContent.Create(actionItem);

        var response = await httpClient.PostAsync("/api/Execution/", content);
        logger.LogDebug("HTTP Response: {Response}", response);

        response.EnsureSuccessStatusCode();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing) _app.Dispose();
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}