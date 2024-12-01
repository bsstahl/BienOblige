using Aspire.Hosting;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.ApiClient;
using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.ApiService.IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest;

[Collection("DistributedApplication")]
public class ActivityInbox_PostActivity_Should
{
    private readonly DistributedApplicationFixture _appFixture;

    public DistributedApplication App => _appFixture.App;

    public ActivityInbox_PostActivity_Should(ITestOutputHelper output, DistributedApplicationFixture appFixture)
    {
        _appFixture = appFixture;
        _appFixture.Configure(output);
    }

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var activityType = Api.Enumerations.ActivityType.Create;
        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build(activityType)
            .Single();

        var correlationId = NetworkIdentity.New().Value;
        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed")
            .Build();

        var activity = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = activityType.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };

        var content = JsonContent.Create(activity);

        var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, content);

        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response Body: {@Response}", body);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task RespondWithTheSpecifiedCorrelationId()
    {
        var correlationGuid = Guid.NewGuid();
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var activityType = Api.Enumerations.ActivityType.Create;
        var correlationId = NetworkIdentity.From(correlationGuid).Value;
        var actionItem = new ActionItemBuilder()
            .UseRandomValues()
            .Id(Guid.NewGuid())
            .Build(activityType)
            .Single();

        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed Company")
            .Build();

        var activity = new Activity()
        {
            Id = NetworkIdentity.New().Value,
            CorrelationId = correlationId,
            ActivityType = activityType.ToString(),
            Actor = updatingActor,
            ActionItem = actionItem
        };
        var content = JsonContent.Create(activity);

        logger.LogInformation("HTTP Request Payload: {@Request}", JsonSerializer.Serialize(activity));

        var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, content);
        var body = await response.Content.ReadAsStringAsync();
        var publicationResult = JsonSerializer.Deserialize<IEnumerable<PublicationResult>>(body);

        logger.LogInformation("HTTP Response: {@Response}", response);

        Assert.Equal(correlationId.ToString(), publicationResult?.Single().Activity?.CorrelationId?.ToString());
    }

    [Fact]
    public async Task RespondWithTheSpecifiedActionItemId()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        using (logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "BienOblige.ApiService.IntegrationTest.ActivityInbox_Post_Should.RespondWithTheSpecifiedActionItemId" }
        }))
        {
            var activityType = Api.Enumerations.ActivityType.Create;
            var correlationId = NetworkIdentity.New().Value;

            var message = new ActivityBuilder()
                .CorrelationId(correlationId)
                .ActivityType(activityType)
                .Actor(new ActorBuilder()
                    .ActorType(Api.Enumerations.ActorType.Organization)
                    .Id(Guid.NewGuid())
                    .Name("Acme Bird Feed"))
                .ActionItem(new ActionItemBuilder()
                    .UseRandomValues())
                .Build();
            var expectedId = message.ActionItem.Id;

            var content = JsonContent.Create(message);

            // Act
            var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, content);
            var body = await response.Content.ReadAsStringAsync();

            // Log the response
            logger.LogInformation("HTTP Response: {@Response}", response);
            var actual = JsonSerializer.Deserialize<IEnumerable<PublicationResult>>(body);
            logger.LogTrace("Publication Results: {@Actual}", actual);

            // Assert
            var actualId = actual!.Single().Activity?.ActionItem.Id;
            Assert.Equal(expected: expectedId, actual: actualId);
        }
    }

    [Fact]
    public async Task ProduceAMessageOnTheActionItemStream()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var correlationId = NetworkIdentity.New().Value;

        var message = new ActivityBuilder()
            .CorrelationId(correlationId)
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .ActorType(Api.Enumerations.ActorType.Organization)
                .Id(Guid.NewGuid())
                .Name(string.Empty.GetRandom()))
            .ActionItem(new ActionItemBuilder()
                .UseRandomValues()
                .Id(Guid.NewGuid()))
            .Build();

        var content = JsonContent.Create(message);

        var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, content);

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

        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(correlationId, Guid.NewGuid(), "Application");

        logger.LogInformation("Request Content: {@Content}", content);
        var response = await httpClient.PostAsync(Api.Constants.Path.ActivityInbox, content);
        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation("HTTP Response: {@Response}", response);

        // TODO: Assert that the stream contains the expected number of messages for that CorrelationId
        // This should probably be done at the DB when the message is consumed by the Execution service
        throw new NotImplementedException();
    }

}