using BienOblige.Api.Builders;
using BienOblige.Api.Test.Extensions;
using BienOblige.Api.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
[Collection("APIClient")]
public class MetroTransitUseCases
{
    const string baseUrl = "https://metrotransit.com";

    private NetworkIdentity complianceStudioServiceId = NetworkIdentity.From(baseUrl, "service", "compliance-studio");

    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public MetroTransitUseCases(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_PublishSinglular_Should>(_config, _output)
            .BuildServiceProvider();
    }

    [Fact]
    public async Task WeeklyDigitalTorqueWrenchCalibration()
    {
        // Arrange
        var activity = new ActivityBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(complianceStudioServiceId)
                .ActorType(Enumerations.ActorType.Service)
                .Name("Compliance Studio"))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Name("Weekly Digital Torque Wrench Calbration")
                .Content("Calibrate the digital torque wrench to a tolerance of 0.5 in-lbs", "text/plain")
                .Audience(new ActorBuilder()
                    .Id(NetworkIdentity.From(baseUrl, "user", "JaneWrencher"))
                    .ActorType(Enumerations.ActorType.Person)
                    .Name("Jane Wrencher"))
                .Target(new ObjectBuilder()
                    .Id(NetworkIdentity.From(baseUrl, "Tool", "digi-torque-124"))
                    .AddObjectType("Object")
                    .Name("Digital Torque Wrench #124")))
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
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Single(actual);
    }

    [Fact]
    public async Task NightlyTasks()
    {
        string vin = "WV3AH4709YH034586";
        string busName = "Bus X-25";
        var busId = NetworkIdentity.From(baseUrl, "bus", vin);
        var departureTime = TimeSpan.FromMinutes(375).GetTimeTomorrowMST();
        var stagingTime = departureTime.AddHours(-1); // Allow at least an hour for staging
        var inspectionTaskId = NetworkIdentity.From(baseUrl, "ActionItem", Guid.NewGuid().ToString());

        var bus = new ObjectBuilder()
            .Id(busId)
            .AddObjectType("schema:Car")
            .AddObjectType("Object")
            .Name(busName)
            .Content($"2023 MetroTransit Type X Bus with VIN *{vin}*", "text/markdown")
            .AddAdditionalProperty("schema:vehicleIdentificationNumber", vin);

        // Arrange
        var activity = new ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name($"{this.GetType().Name}.{nameof(NightlyTasks)}"))
            .ActionItems(new ActionItemCollectionBuilder()
                .Add(new ActionItemBuilder()
                    .Id(inspectionTaskId)
                    .Name("Nightly Inspection")
                    .Content($"Inspect bus {busName} following the Nightly Inspection procedures BKM", MimeType.From("text/plain"))
                    .EndTime(stagingTime) // 05:15 AM Tomorrow
                    .Target(bus))
                .Add(new ActionItemBuilder()
                    .Name("Stage Bus for Next Activity")
                    .Content($"If inspection passed, Stage bus *{busName}* in lane C2 for departure at 06:15 MST. If it failed, stage in the maintenance bay and create a maintenance order.", MimeType.From("text/markdown"))
                    .EndTime(departureTime) // 06:15 AM Tomorrow
                    .AddPrerequisite(inspectionTaskId)
                    .Target(bus)))
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
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Equal(2, actual.Count());

        var inspectionTask = actual.Single(a => a.ActionItem.Id!.Equals(inspectionTaskId.Value.ToString()));
        var dependentTask = actual.Single(a => !a.ActionItem.Id!.Equals(inspectionTaskId.Value.ToString()));

        // Assert that the dependent task has the inspection task's Id in its prerequisite collection
        Assert.Equal(dependentTask.ActionItem.Prerequisites!.Single(), inspectionTask.ActionItem.Id);
    }

}
