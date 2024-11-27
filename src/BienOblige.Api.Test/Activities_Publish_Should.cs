using BienOblige.Api.Builders;
using BienOblige.Api.Targets;
using BienOblige.Api.Test.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
[Collection("APIClient")]
public class Activities_Publish_Should
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public Activities_Publish_Should(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_Publish_Should>(_config, _output)
            .BuildServiceProvider();
    }

    [Fact]
    public async Task PublishASingleActionItem()
    {
        var bus = new Car()
        {
            Id = new Uri("https://bienoblige.com/api/v1/Bus/a986cdac-f35b-448a-a8e3-4118f4d1f6cd"),
            Name = "Bus 1234",
            Description = "2023 MetroTransit Type X Bus with VIN WV3AH4709YH034586",
            Vin = "WV3AH4709YH034586"
        };

        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishASingleActionItem)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Stage Bus for Next Activity")
                .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST")
                .EndTime(TimeSpan.FromMinutes(375).GetTimeTomorrowMST()) // 06:15 AM Tomorrow
                .Target(bus))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient 
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var logger = _services.GetRequiredService<ILogger<Activities_Publish_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
    }

    [Fact]
    public async Task PublishAnActionItemUsingTheObjectBuilderForTheTarget()
    {
        string vin = "WV3AH4709YH034586";

        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishASingleActionItem)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid()) // TODO: Add content
                .Name("Stage Bus for Next Activity")
                .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST")
                .EndTime(TimeSpan.FromMinutes(375).GetTimeTomorrowMST()) // 06:15 AM Tomorrow
                .Target(new ObjectBuilder()
                    .Id(Guid.NewGuid(), "Bus")
                    .AddObjectType("schema:Bus")
                    .AddObjectType("Object")
                    .Name("Bus 1234")
                    .Content($"2023 MetroTransit Type X Bus with VIN *{vin}*", "text/markdown")
                    .AddAdditionalProperty("schema:vehicleIdentificationNumber", vin)))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient 
            ?? throw new InvalidOperationException("No Mock HttpClient found");
        
        var logger = _services.GetRequiredService<ILogger<Activities_Publish_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(vin, actual?.Single()?.ActionItem?.Target?.AdditionalProperties["schema:vehicleIdentificationNumber"].ToString());
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

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activities);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient 
            ?? throw new InvalidOperationException("No Mock HttpClient found");
        var logger = _services.GetRequiredService<ILogger<Activities_Publish_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Equal(4, actual.Count());
    }

    [Fact]
    public async Task ProperlyReturnTheResults()
    {
        // Arrange
        var activities = new ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(ProperlyReturnTheResults)}"))
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
                    .AddAdditionalProperty("shouldThrow", "System.ApplicationException")
                    .Content("ActionItem_3 content"))
                .Add(new ActionItemBuilder()
                    .Name("ActionItem_4")
                    .AddAdditionalProperty("shouldThrow", "System.InvalidOperationException;System.ArgumentNullException")
                    .Content("ActionItem_4 content")))
            .Build();
        
        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activities);

        // Log Results
        var logger = _services.GetRequiredService<ILogger<Activities_Publish_Should>>();
        logger.LogInformation("Publication Results: {@PublicationResults}", JsonSerializer.Serialize(response));

        // Assert - #2 should have an error code, #4 should have thrown an exception
        Assert.True(response.First().SuccessfullyPublished);
        Assert.False(response.Skip(1).First().SuccessfullyPublished);
        Assert.False(response.Skip(2).First().SuccessfullyPublished);
        Assert.False(response.Last().SuccessfullyPublished);
    }
}
