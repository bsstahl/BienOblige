using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
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
public class UseCases
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public UseCases(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_Publish_Should>(_config, _output)
            .BuildServiceProvider();
    }

    [Fact]
    public async Task WeeklyDigitalTorqueWrenchCalibration()
    {
        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(WeeklyDigitalTorqueWrenchCalibration)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Weekly Digital Torque Wrench Calbration")
                .Content("Calibrate the digital torque wrench to a tolerance of 2 in-lbs")
                .Target(new ObjectBuilder()
                    .Id(Guid.NewGuid(), "Tool")
                    .AddObjectType("Object")
                    .Name("Digital Torque Wrench #124")))
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
    public async Task NightlyTasks()
    {
        string vin = "WV3AH4709YH034586";

        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(NightlyTasks)}"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid()) // TODO: Add content
                .Name("Stage Bus for Next Activity")
                .Content("Stage bus 1234 in lane C2 for departure at 06:15 MST")
                .EndTime(TimeSpan.FromMinutes(375).GetTimeTomorrowMST()) // 06:15 AM Tomorrow
                .Target(new ObjectBuilder()
                    .Id(Guid.NewGuid(), "Bus")
                    .AddObjectType("schema:Car")
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

}
