using Aspire.Hosting;
using BienOblige.Api.Builders;
using BienOblige.ApiService.IntegrationTest.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using BienOblige.ApiService.IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.ApiService.IntegrationTest;

[Collection("DistributedApplication")]
public class APIClient_ActivitiesPublish_Should
{
    private readonly DistributedApplicationFixture _appFixture;

    public DistributedApplication App => _appFixture.App;

    public APIClient_ActivitiesPublish_Should(ITestOutputHelper output, DistributedApplicationFixture appFixture)
    {
        _appFixture = appFixture;
        _appFixture.Configure(output);
    }

    [Fact]
    public async Task RespondWithSuccessResults()
    {
        // TODO: Add content
        var content = new Api.Builders.ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new Api.Builders.ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(new Api.Builders.ActionItemCollectionBuilder()
                .Add(new Api.Builders.ActionItemBuilder().UseRandomValues()))
            .Build();

        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(Guid.NewGuid(), Guid.NewGuid(), "Service");
        var client = new ApiClient.Activities(logger, config, httpClient);
        
        var serialized = JsonSerializer.Serialize(content);
        logger.LogInformation("Publishing activities: {@Activities}", serialized);
        var response = await client.Publish(content);
        logger.LogInformation("Response: {@Response}", response);

        Assert.True(response.All(r => r.SuccessfullyPublished));
    }

    [Fact]
    public async Task RespondWithASuccessResultForEachActionItem()
    {
        // TODO: Add content
        var itemCount = 10.GetRandom(3);
        var aiCollectionBuilder = new ActionItemCollectionBuilder();
        for (var i = 0; i < itemCount; i++)
            aiCollectionBuilder.Add(new Api.Builders.ActionItemBuilder()
                .UseRandomValues());

        var content = new Api.Builders.ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new Api.Builders.ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(aiCollectionBuilder)
            .Build();

        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(Guid.NewGuid(), Guid.NewGuid(), "Service");
        var client = new ApiClient.Activities(logger, config, httpClient);

        var serialized = JsonSerializer.Serialize(content);
        logger.LogInformation("Publishing activities: {@Activities}", serialized);
        var response = await client.Publish(content);
        logger.LogInformation("Response: {@Response}", response);

        Assert.Equal(itemCount, response.Count(r => r.SuccessfullyPublished));
    }

    [Fact]
    public async Task CreateAParentChildRelationshipBetweenActionItems()
    {
        // TODO: Add content
        var actionItems = new ActionItemCollectionBuilder()
            .Add(new ActionItemBuilder()
                .UseRandomValues()
                .Children(new ActionItemCollectionBuilder()
                    .Add(new ActionItemBuilder().UseRandomValues())
                ));

        var content = new ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(actionItems)
            .Build();

        var (logger, config, httpClient) = this.App.GetRequiredServices<Controllers.ActivityController>(Guid.NewGuid(), Guid.NewGuid(), "Service");
        var client = new ApiClient.Activities(logger, config, httpClient);

        var serialized = JsonSerializer.Serialize(content);
        logger.LogInformation("Publishing activities: {@Activities}", serialized);
        var response = await client.Publish(content);
        logger.LogInformation("Response: {@Response}", response);

        // The child item's ParentId should be the parent item's Id
        var actualChild = response.Single(r => r.Activity.ActionItem.Parent is not null).Activity.ActionItem;
        var actualParent = response.Single(r => r.Activity.ActionItem.Parent is null).Activity.ActionItem;
        Assert.Equal(actualChild.Parent, actualParent.Id);
    }

}
