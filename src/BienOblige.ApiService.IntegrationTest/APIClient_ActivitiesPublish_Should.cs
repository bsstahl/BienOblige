using Aspire.Hosting;
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
            .Id(Guid.NewGuid())
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


}
