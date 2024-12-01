using BienOblige.Api.Builders;
using BienOblige.Api.Test.Extensions;
using BienOblige.Api.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
[Collection("APIClient")]
public class Activities_PublishCollection_Should
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public Activities_PublishCollection_Should(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_PublishCollection_Should>(_config, _output)
            .BuildServiceProvider();
    }

    [Fact]
    public async Task PublishActionItemsUsingTheObjectBuilderForTheTarget()
    {
        var vin = "WV3AH4709YH034586";
        var dependentTaskId = NetworkIdentity.From("http://example.com", "ActionItem", Guid.NewGuid().ToString());
        var stageByTime = TimeSpan.FromMinutes(375).GetTimeTomorrowMST();

        var targetBusBuilder = new ObjectBuilder()
            .Id(Guid.NewGuid(), "Bus")
            .AddObjectType("schema:Bus")
            .AddObjectType("Object")
            .Name("Bus 1234")
            .Content($"2023 MetroTransit Type X Bus with VIN *{vin}*", "text/markdown")
            .AddAdditionalProperty("schema:vehicleIdentificationNumber", vin);

        // Arrange
        var activity = new ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishActionItemsUsingTheObjectBuilderForTheTarget)}"))
            .ActionItems(new ActionItemCollectionBuilder()
                .Add(new ActionItemBuilder() // TODO: Add prerequisite task (id is stored in dependentTaskId)
                    .Id(Guid.NewGuid())
                    .Name("Stage Bus for Next Activity")
                    .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST")
                    .EndTime(stageByTime) // 06:15 AM Tomorrow
                    .Target(targetBusBuilder))
                .Add(new ActionItemBuilder()
                    .Id(dependentTaskId)
                    .Name("Perform daily inspection")
                    .Content("Perform daily inspection of bus 1234 prior to staging")
                    .EndTime(stageByTime.AddHours(-1)) // Allow at least an hour for staging
                    .Target(targetBusBuilder)))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient 
            ?? throw new InvalidOperationException("No Mock HttpClient found");
        
        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: {@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.All(r => r.SuccessfullyPublished));

        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        
        var vins = actual.Select(a => a.ActionItem?.Target?.AdditionalProperties["schema:vehicleIdentificationNumber"].ToString());
        Assert.Equal(2, vins.Count());
        Assert.Single(vins.Distinct());
        Assert.Equal(vin, vins.Distinct().Single());
    }

    [Fact]
    public async Task ProduceTheExpectedNumberOfActivities()
    {
        // Arrange
        var activities = new ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(ProduceTheExpectedNumberOfActivities)}"))
            .ActionItems(new ActionItemCollectionBuilder()
                .Add(new ActionItemBuilder()
                    // TODO: Add content
                    .Id(Guid.NewGuid()) // TODO: Add content
                    .Name("ActionItem_1")
                    .Content("ActionItem_1 content")
                    .AddCompletionMethod(Enumerations.CompletionMethod.AllChildrenCompleted)
                    .Children(new ActionItemCollectionBuilder()
                        .Add(new ActionItemBuilder()
                            .Id(Guid.NewGuid()) // TODO: Add content
                            .Name("ActionItem_2")
                            .Content("ActionItem_2: A child task of ActionItem_1")
                            .AddCompletionMethod(Enumerations.CompletionMethod.ParentCompleted)
                            .AddCompletionMethod(Enumerations.CompletionMethod.AnyChildCompleted)
                            .Children(new ActionItemCollectionBuilder()
                                .Add(new ActionItemBuilder()
                                    .Name("ActionItem_3")
                                    .Content("ActionItem_3: A child task of ActionItem_2")
                                    .AddCompletionMethod(Enumerations.CompletionMethod.ParentCompleted))
                                .Add(new ActionItemBuilder()
                                    .Name("ActionItem_4")
                                    .Content("ActionItem_4: A child task of ActionItem_2")
                                    .AddCompletionMethod(Enumerations.CompletionMethod.ParentCompleted))
                    )))))
            .Build();

        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();

        using (logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "BienOblige.Api.Test.Activities_Publish_Should.ProduceTheExpectedNumberOfActivities" }
        }))
        {
            // Act
            var client = _services.GetRequiredService<ApiClient.Activities>();
            var response = await client.Publish(activities);

            // Log Activity
            var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
                ?? throw new InvalidOperationException("No Mock HttpClient found");
            logger.LogInformation("Activity Request: {@Activities}", httpClient.JsonRequestMessages);

            // Assert
            Assert.True(response.All(p => p.SuccessfullyPublished));

            var actual = httpClient.ActivityRequests;
            Assert.NotNull(actual);
            Assert.Equal(4, actual.Count());
        }   
    }

    [Fact]
    public async Task ProperlyReturnTheIndividualResults()
    {
        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();

        using (logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "BienOblige.Api.Test.Activities_Publish_Should.ProperlyReturnTheResults" }
        }))
        {
            // Arrange
            var activities = new ActivitiesCollectionBuilder()
                .CorrelationId(Guid.NewGuid())
                .ActivityType(Api.Enumerations.ActivityType.Create)
                .Actor(new ActorBuilder()
                    .Id(Guid.NewGuid())
                    .ActorType(Api.Enumerations.ActorType.Application)
                    .Name($"{this.GetType().Name}.{nameof(ProperlyReturnTheIndividualResults)}"))
                .ActionItems(new ActionItemCollectionBuilder()
                    .Add(new ActionItemBuilder()
                        .Id(Guid.NewGuid())
                        .Name("ActionItem_1")
                        .Content("ActionItem_1 content"))
                    .Add(new ActionItemBuilder()
                        .Id(Guid.NewGuid())
                        .AddAdditionalProperty("shouldFail", true)
                        .Name("ActionItem_2")
                        .Content("ActionItem_2 content"))
                    .Add(new ActionItemBuilder()
                        .Name("ActionItem_3")
                        .AddAdditionalProperty("shouldFail", true)
                        .Content("ActionItem_3 content"))
                    .Add(new ActionItemBuilder()
                        .Name("ActionItem_4")
                        .Content("ActionItem_4 content")))
                .Build();

            logger.LogInformation("Activities: {Activities}", JsonSerializer.Serialize(activities));

            // Act
            var client = _services.GetRequiredService<ApiClient.Activities>();
            var response = await client.Publish(activities);

            // Log Results
            logger.LogInformation("Publication Results: {@PublicationResults}", JsonSerializer.Serialize(response));

            // Assert - #2 & #3 should have an error code
            Assert.True(response.First().SuccessfullyPublished);
            Assert.False(response.Skip(1).First().SuccessfullyPublished);
            Assert.False(response.Skip(2).First().SuccessfullyPublished);
            Assert.True(response.Last().SuccessfullyPublished);
        }   
    }

    [Fact]
    public async Task ProperlyReturnTheFailureResultsForAllActivities()
    {
        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();

        using (logger.BeginScope(new Dictionary<string, object>
        {
            { "Method", "BienOblige.Api.Test.Activities_Publish_Should.ProperlyReturnTheFailureResultsForAllActivities" }
        }))
        {
            // Arrange
            var activities = new ActivitiesCollectionBuilder()
                .CorrelationId(Guid.NewGuid())
                .ActivityType(Api.Enumerations.ActivityType.Create)
                .AddAdditionalProperty("shouldThrow", true)
                .Actor(new ActorBuilder()
                    .Id(Guid.NewGuid())
                    .ActorType(Api.Enumerations.ActorType.Application)
                    .Name($"{this.GetType().Name}.{nameof(ProperlyReturnTheFailureResultsForAllActivities)}"))
                .ActionItems(new ActionItemCollectionBuilder()
                    .Add(new ActionItemBuilder()
                        .Id(Guid.NewGuid())
                        .Name("ActionItem_1")
                        .Content("ActionItem_1 content"))
                    .Add(new ActionItemBuilder()
                        .Id(Guid.NewGuid())
                        .Name("ActionItem_2")
                        .Content("ActionItem_2 content"))
                    .Add(new ActionItemBuilder()
                        .Name("ActionItem_3")
                        .Content("ActionItem_3 content"))
                    .Add(new ActionItemBuilder()
                        .Name("ActionItem_4")
                        .Content("ActionItem_4 content")))
                .Build();

            logger.LogInformation("Activities: {Activities}", JsonSerializer.Serialize(activities));

            // Act
            var client = _services.GetRequiredService<ApiClient.Activities>();
            var response = await client.Publish(activities);

            // Log Results
            logger.LogInformation("Publication Results: {@PublicationResults}", JsonSerializer.Serialize(response));

            // Assert - All should have an error code
            Assert.True(response.All(r => !r.SuccessfullyPublished));
        }
    }
}
