using Aspire.Hosting;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Messages;
using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.ApiService.IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest;

[Collection("DistributedApplication")]
public class ActivityInbox_Post_Should
{
    private readonly DistributedApplicationFixture _appFixture;

    public DistributedApplication App => _appFixture.App;

    public ActivityInbox_Post_Should(ITestOutputHelper output, DistributedApplicationFixture appFixture)
    { 
        _appFixture = appFixture;
        _appFixture.Configure(output);
    }

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_Post_Should>(Guid.NewGuid(), Guid.NewGuid(), "Service");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build()
            .Single();

        var correlationId = NetworkIdentity.New().Value;
        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed")
            .Build();

        var message = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = Api.Enumerations.ActivityType.Create.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };

        var content = JsonContent.Create(message);

        var response = await httpClient.PostAsync(Api.Constants.Path.Inbox, content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response Body: {@Response}", body);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task RespondWithTheSpecifiedCorrelationId()
    {
        var correlationGuid = Guid.NewGuid();
        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(correlationGuid, Guid.NewGuid(), "Person");

        var correlationId = NetworkIdentity.From(correlationGuid).Value;
        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build()
            .Single();

        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed Company")
            .Build();

        var message = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = Api.Enumerations.ActivityType.Create.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };
        var content = JsonContent.Create(message);

        logger.LogInformation("HTTP Request Payload: {@Request}", JsonSerializer.Serialize(message));

        var response = await httpClient.PostAsync(Api.Constants.Path.Inbox, content);
        var body = await response.Content.ReadAsStringAsync();
        var publicationResponse = JsonSerializer.Deserialize<ActivityPublicationResponse>(body);

        logger.LogInformation("HTTP Response: {@Response}", response);

        Assert.Equal(correlationId.ToString(), publicationResponse?.CorrelationId);
    }

    [Fact]
    public async Task RespondWithTheSpecifiedActionItemId()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(Guid.NewGuid(), Guid.NewGuid(), "Organization");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build()
            .Single();

        var correlationId = NetworkIdentity.New().Value;
        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed")
            .Build();

        var message = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = Api.Enumerations.ActivityType.Create.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };
        var content = JsonContent.Create(message);

        var response = await httpClient.PostAsync(Api.Constants.Path.Inbox, content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        var actual = JsonSerializer.Deserialize<ActivityPublicationResponse>(body);
        var actualId = actual!.ActionItemId;
        
        Assert.Equal(actionItem.Id?.ToString(), actualId);
    }

    [Fact]
    public async Task ProduceAMessageOnTheActionItemStream()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build()
            .Single();

        var correlationId = NetworkIdentity.New().Value;
        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name(string.Empty.GetRandom())
            .Build();

        var message = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = Api.Enumerations.ActivityType.Create.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };
        var content = JsonContent.Create(message);

        var response = await httpClient.PostAsync(Api.Constants.Path.Inbox, content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ProduceAConsumableMessageOnTheActivityStream()
    {
        var correlationId = Guid.NewGuid();

        var itemCount = 10.GetRandom(3);
        var actionItems = new ActionItemCollectionBuilder();
        for (var i = 0; i < itemCount; i++)
            actionItems.Add(new ActionItemBuilder()
                .UseRandomValues()
                .Id(Guid.NewGuid()));

        var activities = new ActivitiesCollectionBuilder()
            .CorrelationId(correlationId)
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"ActorName: {string.Empty.GetRandom()}"))
            .ActionItems(actionItems)
            .Build();

        var content = JsonContent.Create(actionItems);

        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(correlationId, Guid.NewGuid(), "Application");

        logger.LogInformation("Request Content: {@Content}", content);
        var response = await httpClient.PostAsync(Api.Constants.Path.Inbox, content);
        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        // TODO: Assert that the stream contains the expected number of messages for that CorrelationId
        // This should probably be done at the DB when the message is consumed by the Execution service
        throw new NotImplementedException();
    }

}