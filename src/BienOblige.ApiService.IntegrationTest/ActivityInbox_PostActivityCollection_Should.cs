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
public class ActivityInbox_PostActivityCollection_Should
{
    private readonly DistributedApplicationFixture _appFixture;

    public DistributedApplication App => _appFixture.App;

    public ActivityInbox_PostActivityCollection_Should(ITestOutputHelper output, DistributedApplicationFixture appFixture)
    {
        _appFixture = appFixture;
        _appFixture.Configure(output);
    }

    [Fact]
    public async Task RespondWithAnAcceptedResult()
    {
        var (logger, config, httpClient) = this.App.GetRequiredServices<ActivityInbox_PostActivity_Should>(Guid.NewGuid(), Guid.NewGuid(), "Group");

        var activityType = Api.Enumerations.ActivityType.Create;
        var correlationId = NetworkIdentity.New().Value;

        var actionItem = new ActionItemCollectionBuilder()
            .Add(new ActionItemBuilder().UseRandomValues());

        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed");

        var activity = new ActivitiesCollectionBuilder()
            .CorrelationId(correlationId)
            .ActivityType(activityType)
            .Actor(updatingActor)
            .ActionItems(actionItem)
            .Build();

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

        var actionItem = new ActionItemCollectionBuilder()
            .Add(new ActionItemBuilder().UseRandomValues());

        var updatingActor = new ActorBuilder()
            .ActorType(Api.Enumerations.ActorType.Organization)
            .Id(Guid.NewGuid())
            .Name("Acme Bird Feed");

        var activity = new ActivitiesCollectionBuilder()
            .CorrelationId(correlationId)
            .ActivityType(activityType)
            .Actor(updatingActor)
            .ActionItems(actionItem)
            .Build();

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

            var message = new ActivitiesCollectionBuilder()
                .CorrelationId(correlationId)
                .ActivityType(activityType)
                .Actor(new ActorBuilder()
                    .ActorType(Api.Enumerations.ActorType.Organization)
                    .Id(Guid.NewGuid())
                    .Name("Acme Bird Feed"))
                .ActionItems(new ActionItemCollectionBuilder()
                    .Add(new ActionItemBuilder().UseRandomValues()))
                .Build();
            var expectedId = message.Single().ActionItem.Id;

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

}