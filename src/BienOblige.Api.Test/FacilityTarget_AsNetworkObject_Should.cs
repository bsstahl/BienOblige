using BienOblige.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class FacilityTarget_AsNetworkObject_Should
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public FacilityTarget_AsNetworkObject_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<FacilityTarget_AsNetworkObject_Should>>();
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperId()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.Id, actual.Id);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperName()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.Name, actual.Name);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperDescription()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.Description, actual.Content);
        Assert.Equal("text/plain", actual.MediaType?.ToString());
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperSummary()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.Summary, actual.Summary);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperPropertyType()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.PropertyType, actual.AdditionalProperties["schema:PropertyType"]);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperAddress()
    {
        var facility = GetRandomFacility(_logger);
        var actual = facility.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(facility.Address, actual.AdditionalProperties["schema:Address"]);
    }

    [Fact]
    public void RoundTripSerializationSuccessfully()
    {
        var entity = GetRandomFacility(_logger);
        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Id, deserialized?.Id);
    }

    private static Targets.Facility GetRandomFacility(ILogger logger)
    {
        var uid = Guid.NewGuid();
        var url = new Uri($"https://example.com/residence/{uid}");
        var streetAddress = string.Empty.GetRandomUSAddress();
        var (city, state) = TestHelpers.CityPairs.GetRandom();
        var zip = $"{99.GetRandom(10):00}{999.GetRandom():000}";
        var address = $"{streetAddress}, {city} {state} {zip}";
        var resoPropertyType = TestHelpers.ResoCodes.GetRandom();

        var facility = new Targets.Facility()
        {
            Id = url,
            Name = $"Facility at {address}",
            Address = address,
            PropertyType = resoPropertyType,
            Description = $"A commercial property of type {resoPropertyType} located at {streetAddress} in {city} {state})",
            Summary = $"A potentially AI generated summary describing the property and circumstances of the facility at {streetAddress}",
        };

        logger.LogTrace("Facility: {@Facility}", facility);
        return facility;
    }

}
