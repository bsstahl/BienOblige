using Aspire.Hosting;
using BienOblige.ApiService.Entities;
using BienOblige.ApiService.Extensions;
using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.ApiService.IntegrationTest.Fixtures;
using BienOblige.Execution.Builders;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest;

[Collection("DistributedApplication")]
public class Execution_Create_Should
{
    private readonly DistributedApplicationFixture _appFixture;

    public DistributedApplication App => _appFixture.App;

    public Execution_Create_Should(ITestOutputHelper output, DistributedApplicationFixture appFixture)
    { 
        _appFixture = appFixture;
        _appFixture.Configure(output);
    }

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var (logger, httpClient) = this.App.GetRequiredServices<Controllers.ExecutionController>(Guid.NewGuid(), Guid.NewGuid(), "Service");

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
        var (logger, httpClient) = this.App.GetRequiredServices<Controllers.ExecutionController>(expectedCorrelationId, Guid.NewGuid(), "Person");

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
        var (logger, httpClient) = this.App.GetRequiredServices<Controllers.ExecutionController>(Guid.NewGuid(), Guid.NewGuid(), "Organization");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var content = actionItem.AsJsonContent();

        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        var actual = JsonSerializer.Deserialize<CreateResponse>(body);
        var actualId = actual!.Ids.Single();
        
        Assert.Equal(actionItem.Id.Value.ToString(), actualId);
    }

    [Fact]
    public async Task ProduceAMessageOnTheActionItemStream()
    {
        var (logger, httpClient) = this.App.GetRequiredServices<Controllers.ExecutionController>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var content = new ActionItemBuilder()
            .UseRandomValues()
            .BuildJsonContent();

        var response = await httpClient.PostAsync("/api/Execution/", content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ProduceOneMessageOnTheActionItemStreamPerActionItem()
    {
        var correlationId = Guid.NewGuid();
        var (logger, httpClient) = this.App.GetRequiredServices<Controllers.ExecutionController>(correlationId, Guid.NewGuid(), "Application");

        var itemCount = 10.GetRandom(3);
        var actionItems = new List<Execution.Aggregates.ActionItem>();
        for (var i = 0; i < itemCount; i++)
            actionItems.Add(new ActionItemBuilder().UseRandomValues().Build());
        var contents = JsonContent.Create(actionItems);

        var response = await httpClient.PostAsync("/api/Execution/", contents);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        // TODO: Assert that the stream contains the expected number of messages for that CorrelationId
        // This should probably be done at the DB when the message is consumed by the Execution service
    }

}