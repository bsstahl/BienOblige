using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;
using BienOblige.Api.Samples;
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
public class MaintenanceActionItem_Should
{
    private const string BaseUrl = "https://metrotransit.com";
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public MaintenanceActionItem_Should(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder().Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_PublishSinglular_Should>(_config, _output)
            .BuildServiceProvider();
        _logger = _services.GetRequiredService<ILogger<MaintenanceActionItem_Should>>();
    }

    [Fact]
    public void PopulateCustomFieldsAndTypesCorrectly()
    {
        var item = new MaintenanceActionItem
        {
            Id = new Uri($"{BaseUrl}/ActionItem/{Guid.NewGuid()}"),
            Name = "Pneumatic Brake System Inspection",
            Content = "Inspect and calibrate the pneumatic brake lines and pressure valves on Bus 4022",
            WorkOrderNumber = "WO-2025-0842",
            EstimatedCost = 350.00m,
            RequiresCertification = true,
            RegulatoryAuthority = "DOT"
        };

        Assert.Contains(MaintenanceActionItem.MaintenanceObjectTypeName, item.ObjectType);
        Assert.Contains("bienoblige:ActionItem", item.ObjectType);
        Assert.Contains("Object", item.ObjectType);
        Assert.Equal("WO-2025-0842", item.WorkOrderNumber);
        Assert.Equal(350.00m, item.EstimatedCost);
        Assert.True(item.RequiresCertification);
        Assert.Equal("DOT", item.RegulatoryAuthority);
    }

    [Fact]
    public void ConvertToNetworkObjectPreservingCustomProperties()
    {
        var item = new MaintenanceActionItem
        {
            Id = new Uri($"{BaseUrl}/ActionItem/{Guid.NewGuid()}"),
            Name = "Air Compressor Overhaul",
            Content = "Perform scheduled rebuild of the main air compressor unit",
            WorkOrderNumber = "WO-2025-0915",
            EstimatedCost = 750.50m,
            RequiresCertification = true,
            RegulatoryAuthority = "FTA"
        };

        var networkObject = item.AsNetworkObject();

        Assert.Equal(item.Id, networkObject.Id);
        Assert.Equal(item.Name, networkObject.Name);
        Assert.Equal(item.Content, networkObject.Content);
        Assert.Equal(item.ObjectType, networkObject.ObjectType);

        Assert.True(networkObject.AdditionalProperties.ContainsKey("metrotransit:workOrderNumber"));
        Assert.Equal("WO-2025-0915", networkObject.AdditionalProperties["metrotransit:workOrderNumber"].ToString());

        Assert.True(networkObject.AdditionalProperties.ContainsKey("metrotransit:estimatedCost"));
        Assert.True(networkObject.AdditionalProperties.ContainsKey("metrotransit:requiresCertification"));
        Assert.True(networkObject.AdditionalProperties.ContainsKey("metrotransit:regulatoryAuthority"));
        Assert.Equal("FTA", networkObject.AdditionalProperties["metrotransit:regulatoryAuthority"].ToString());
    }

    [Fact]
    public void RoundTripSerializationViaNetworkObjectGenericExtension()
    {
        var original = new MaintenanceActionItem
        {
            Id = new Uri($"{BaseUrl}/ActionItem/{Guid.NewGuid()}"),
            Name = "Emergency Exit Door Sensor Test",
            Content = "Verify functional operation and alarm triggering of all emergency exit sensors",
            WorkOrderNumber = "WO-2025-1002",
            EstimatedCost = 125.00m,
            RequiresCertification = false,
            RegulatoryAuthority = "OSHA"
        };

        var networkObject = original.AsNetworkObject();
        var deserialized = networkObject.AsActionItem<MaintenanceActionItem>();

        Assert.NotNull(deserialized);
        Assert.Equal(original.Id, deserialized.Id);
        Assert.Equal(original.Name, deserialized.Name);
        Assert.Equal(original.Content, deserialized.Content);
        Assert.Equal(original.WorkOrderNumber, deserialized.WorkOrderNumber);
        Assert.Equal(original.EstimatedCost, deserialized.EstimatedCost);
        Assert.Equal(original.RequiresCertification, deserialized.RequiresCertification);
        Assert.Equal(original.RegulatoryAuthority, deserialized.RegulatoryAuthority);
    }

    [Fact]
    public async Task PublishActivityWithCustomActionItemAndContext()
    {
        // Arrange
        var complianceStudioId = NetworkIdentity.From(BaseUrl, "service", "compliance-studio");
        var busTarget = new ObjectBuilder()
            .Id(NetworkIdentity.From(BaseUrl, "bus", "BUS-4022"))
            .AddObjectType("schema:Car")
            .AddObjectType("Object")
            .Name("MetroTransit Bus #4022");

        var customActionItem = new MaintenanceActionItem
        {
            Id = new Uri($"{BaseUrl}/ActionItem/{Guid.NewGuid()}"),
            Name = "Annual Pneumatic Brake System Certification",
            Content = "Perform comprehensive diagnostic test and pressure tolerance certification of braking system",
            WorkOrderNumber = "WO-2025-0842",
            EstimatedCost = 450.00m,
            RequiresCertification = true,
            RegulatoryAuthority = "DOT",
            Target = busTarget.Build(),
            Audience = new ActorBuilder()
                .Id(NetworkIdentity.From(BaseUrl, "user", "JaneWrencher"))
                .ActorType(ActorType.Person)
                .Name("Jane Wrencher")
                .Build().AsNetworkObject()
        };

        var activity = new CreateActionItemActivityBuilder(new Uri(BaseUrl))
            .CorrelationId(Guid.NewGuid())
            .AddContext(MaintenanceActionItem.ExtensionPrefix, MaintenanceActionItem.ExtensionNamespace)
            .Actor(new ActorBuilder()
                .Id(complianceStudioId)
                .ActorType(ActorType.Service)
                .Name("Compliance Studio"))
            .ActionItem(customActionItem)
            .Build();

        // Act
        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        // Verify Mock HttpClient captured request
        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var jsonMessage = httpClient.JsonRequestMessages;
        _logger.LogInformation("Generated Activity JSON:\r\n{Json}", jsonMessage);

        // Assert
        var requests = httpClient.ActivityRequests;
        Assert.NotNull(requests);
        Assert.Single(requests);

        // Verify JSON includes custom context and properties
        using var doc = JsonDocument.Parse(jsonMessage);
        var root = doc.RootElement.ValueKind == JsonValueKind.Array
            ? doc.RootElement[0]
            : doc.RootElement;

        // Context check
        var contextElement = root.GetProperty("@context");
        Assert.Equal(JsonValueKind.Array, contextElement.ValueKind);
        bool hasCustomContext = false;
        foreach (var ctx in contextElement.EnumerateArray())
        {
            if (ctx.ValueKind == JsonValueKind.Object && ctx.TryGetProperty("metrotransit", out var uriProp))
            {
                if (uriProp.GetString() == "https://metrotransit.com/ns")
                    hasCustomContext = true;
            }
        }
        Assert.True(hasCustomContext, "Activity should include custom 'metrotransit' context namespace");

        // Object check
        var objectElement = root.GetProperty("object");
        Assert.Equal("WO-2025-0842", objectElement.GetProperty("metrotransit:workOrderNumber").GetString());
        Assert.Equal(450.00m, objectElement.GetProperty("metrotransit:estimatedCost").GetDecimal());
        Assert.True(objectElement.GetProperty("metrotransit:requiresCertification").GetBoolean());
        Assert.Equal("DOT", objectElement.GetProperty("metrotransit:regulatoryAuthority").GetString());

        // Object type check
        var typeArray = objectElement.GetProperty("@type");
        var types = typeArray.EnumerateArray().Select(t => t.GetString()).ToList();
        Assert.Equal(["metrotransit:MaintenanceActionItem", "bienoblige:ActionItem", "Object"], types);
    }

    [Fact]
    public async Task PublishActivityWithActionItemBuilderExtensionProperties()
    {
        // Demonstrates using ActionItemBuilder with AddObjectType and AddAdditionalProperty
        var complianceStudioId = NetworkIdentity.From(BaseUrl, "service", "compliance-studio");

        var activity = new CreateActionItemActivityBuilder(new Uri(BaseUrl))
            .CorrelationId(Guid.NewGuid())
            .AddContext("metrotransit", "https://metrotransit.com/ns")
            .Actor(new ActorBuilder()
                .Id(complianceStudioId)
                .ActorType(ActorType.Service)
                .Name("Compliance Studio"))
            .ActionItem(new ActionItemBuilder(new Uri(BaseUrl))
                .Id(Guid.NewGuid())
                .AddObjectType("metrotransit:MaintenanceActionItem")
                .Name("Wheel Alignment Verification")
                .Content("Check front-axle wheel alignment following curb strike report", "text/plain")
                .AddAdditionalProperty("metrotransit:workOrderNumber", "WO-2025-0999")
                .AddAdditionalProperty("metrotransit:estimatedCost", 175.50m)
                .AddAdditionalProperty("metrotransit:requiresCertification", false)
                .AddAdditionalProperty("metrotransit:regulatoryAuthority", "Internal"))
            .Build();

        var client = _services.GetRequiredService<ApiClient.Activities>();
        var response = await client.Publish(activity);

        var httpClient = _services.GetRequiredService<Mocks.MockHttpClient>() as Mocks.MockHttpClient
            ?? throw new InvalidOperationException("No Mock HttpClient found");

        var jsonMessage = httpClient.JsonRequestMessages;
        _logger.LogInformation("Generated Builder JSON:\r\n{Json}", jsonMessage);

        using var doc = JsonDocument.Parse(jsonMessage);
        var root = doc.RootElement.ValueKind == JsonValueKind.Array
            ? doc.RootElement[doc.RootElement.GetArrayLength() - 1]
            : doc.RootElement;
        var obj = root.GetProperty("object");
        Assert.Equal("WO-2025-0999", obj.GetProperty("metrotransit:workOrderNumber").GetString());
        Assert.Equal(175.50m, obj.GetProperty("metrotransit:estimatedCost").GetDecimal());
        Assert.False(obj.GetProperty("metrotransit:requiresCertification").GetBoolean());
        Assert.Equal("Internal", obj.GetProperty("metrotransit:regulatoryAuthority").GetString());
    }

    [Fact]
    public void PreserveDefaultContextsWhenUsingCollectionAddContext()
    {
        var customContexts = new List<KeyValuePair<string?, string>>
        {
            new("metrotransit", "https://metrotransit.com/ns"),
            new("customns", "https://example.com/ns")
        };

        var activity = new CreateActionItemActivityBuilder(new Uri(BaseUrl))
            .CorrelationId(Guid.NewGuid())
            .Actor(new ActorBuilder()
                .Id(new Uri($"{BaseUrl}/actor/1"))
                .ActorType(ActorType.Service)
                .Name("Test Service"))
            .AddContext(customContexts)
            .ActionItem(new ActionItemBuilder(new Uri(BaseUrl))
                .Id(new Uri($"{BaseUrl}/item/1"))
                .Name("Test Item")
                .Content("Test Content", "text/plain"))
            .Build();

        Assert.NotNull(activity.Context);
        // Default contexts should be preserved: AS2, BienOblige, Schema
        Assert.Contains(activity.Context, c => c.Value == "https://www.w3.org/ns/activitystreams");
        Assert.Contains(activity.Context, c => c.Key == "bienoblige" && c.Value == "https://bienoblige.com/ns");
        Assert.Contains(activity.Context, c => c.Key == "schema" && c.Value == "https://schema.org");
        // Custom contexts should be present
        Assert.Contains(activity.Context, c => c.Key == "metrotransit" && c.Value == "https://metrotransit.com/ns");
        Assert.Contains(activity.Context, c => c.Key == "customns" && c.Value == "https://example.com/ns");
    }
}
