using BienOblige.Api.Builders;
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
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .AddAdditionalProperty("shouldThrow", true)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(ReportTheErrorIfAnHttpErrorOccurs)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Action item name")
                .Content("ActionItemContent"))
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
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishAnActionItemUsingTheObjectBuilderForTheTarget)}"))
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
        
        var logger = _services.GetRequiredService<ILogger<Activities_PublishSinglular_Should>>();
        logger.LogInformation("Activity Request: {@Activities}", httpClient.JsonRequestMessages);

        // Assert
        Assert.True(response.SuccessfullyPublished);

        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(vin, actual?.Single()?.ActionItem?.Target?.AdditionalProperties["schema:vehicleIdentificationNumber"].ToString());
    }

    [Fact]
    public async Task PublishAValidLocationAssignmentMessage()
    {
        var actionItemId = NetworkIdentity.From("https://example.com", "ActionItem", "fd1cf331-c12d-4840-a197-ea2b08ddd240");
        var locationId = NetworkIdentity.From("https://example.com", "Location", "94f6cf65-568f-49b7-a501-95e5e9371c6a");

        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Update)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(PublishAValidLocationAssignmentMessage)}"))
            .AssignToLocation(actionItemId, new LocationBuilder()
                .Id(locationId)
                .Name("The company's Phoenix AZ location"))
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
        Assert.Equal(locationId.ToString(), actual.Single().ActionItem.Location?.ObjectId.ToString());
    }
}
