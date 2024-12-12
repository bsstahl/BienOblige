using BienOblige.Api.Builders;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;
using BienOblige.Api.Targets;
using BienOblige.Api.Test.Extensions;
using BienOblige.Api.Test.Mocks;
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
public class Activities_PublishSinglular_Should
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public Activities_PublishSinglular_Should(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_PublishSinglular_Should>(_config, _output)
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
        var activity = new CreateActionItemActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishASingleActionItem)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Stage Bus for Next Activity")
                .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST", "text/plain")
                .EndTime(TimeSpan.FromMinutes(375).GetTimeTomorrowMST()) // 06:15 AM Tomorrow
                .Target(bus))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient 
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.SuccessfullyPublished);

        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(activity.Id, actual.Single().Id);
    }

    [Fact]
    public async Task ReportTheErrorIfAnHttpErrorOccurs()
    {
        // Arrange
        var activity = new CreateActionItemActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .AddAdditionalProperty("shouldThrow", true)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(ReportTheErrorIfAnHttpErrorOccurs)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Action item name")
                .Content("ActionItemContent", "text/plain"))
            .Build();

        var logger = _services.GetRequiredService<ILogger<ApiClient.Activities>>();

        using (logger.BeginScope(new Dictionary<string, object>()
            {
                {  "Method", "BienOblige.Api.Test.Activities_PublishSingular_Should.ReportTheErrorIfAnHttpErrorOccurs" }
            }))
        {
            var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
                ?? throw new InvalidOperationException("No Mock HttpClient found");

            // TODO: Implement test

            // Act
            var client = _services.GetRequiredService<ApiClient.Activities>();
            var response = await client.Publish(activity);

            // Log Activity

            logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

            // Assert
            Assert.False(response.SuccessfullyPublished);
            Assert.True(response.Errors.Any());
        }
    }

    [Fact]
    public async Task PublishAnActionItemUsingTheObjectBuilderForTheTarget()
    {
        string vin = "WV3AH4709YH034586";

        // Arrange
        var activity = new CreateActionItemActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishAnActionItemUsingTheObjectBuilderForTheTarget)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid()) // TODO: Add content
                .Name("Stage Bus for Next Activity")
                .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST", "text/plain")
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
        
        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: {@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.SuccessfullyPublished);

        var actualActivities = httpClient.ActivityRequests;
        var actualActionItem = actualActivities?.Single()?.Object?.AsActionItem()
            ?? throw new ArgumentNullException("ActionItem");
        var actualVin = actualActionItem.Target?.AdditionalProperties["schema:vehicleIdentificationNumber"];

        Assert.NotNull(actualActivities);
        Assert.Single(actualActivities);
        Assert.Equal(vin, actualVin?.ToString());
    }

    [Fact]
    public async Task PublishAValidLocationAssignmentByActionItemMessage()
    {
        const string baseUri = "https://example.com";

        var actionItemId = NetworkIdentity.From(baseUri, "ActionItem", "fd1cf331-c12d-4840-a197-ea2b08ddd240");
        var locationId = NetworkIdentity.From(baseUri, "Location", "94f6cf65-568f-49b7-a501-95e5e9371c6a");

        // Arrange
        var activity = new UpdateLocationActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .Actor(new ActorBuilder()
                .Id(NetworkIdentity.From(baseUri, "Service", Guid.NewGuid().ToString()))
                .ActorType(Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishAValidLocationAssignmentByActionItemMessage)}"))
            .Location(new LocationBuilder()
                .Id(locationId)
                .Name("The company's Phoenix AZ location"))
            .Target(new ObjectIdentifierBuilder()
                .Id(actionItemId)
                .AddObjectType("bienoblige:ActionItem"))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.SuccessfullyPublished);

        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(ActivityType.Update.ToString(), activity.ActivityType);

        var actualLocation = actual.Single().Object;
        Assert.Equal(locationId.ToString(), actualLocation.Id.ToString());

        var actualTarget = actual.Single().Target;
        Assert.Equal(actionItemId.ToString(), actualTarget.Id.ToString());
    }

    [Fact]
    public async Task PublishAValidLocationAssignmentByTagMessage()
    {
        const string baseUri = "https://example.com";

        var locationId = NetworkIdentity.From(baseUri, "Location", "SWRegionalHQ");
        var tagId = NetworkIdentity.From(baseUri, "SpecialCare", "WhiteGlove");

        // Arrange
        var activity = new UpdateLocationActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .Actor(new ActorBuilder()
                .Id(NetworkIdentity.From(baseUri, "Service", Guid.NewGuid().ToString()))
                .ActorType(Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishAValidLocationAssignmentByTagMessage)}"))
            .Location(new LocationBuilder()
                .Id(locationId)
                .Name("The company's Phoenix AZ location"))
            .Target(new ObjectIdentifierBuilder()
                .Id(tagId)
                .AddObjectType("example:SpecialCare")
                .AddObjectType("Object"))
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Log Activity
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.SuccessfullyPublished);

        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(ActivityType.Update.ToString(), actual.Single().ActivityType);

        var actualLocation = actual.Single().Object;
        Assert.Equal(locationId.ToString(), actualLocation.Id.ToString());

        var actualTarget = actual.Single().Target;
        Assert.Equal(tagId.ToString(), actualTarget.Id.ToString());
    }
}
