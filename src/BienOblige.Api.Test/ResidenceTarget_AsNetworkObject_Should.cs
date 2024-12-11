using BienOblige.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class ResidenceTarget_AsNetworkObject_Should
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public ResidenceTarget_AsNetworkObject_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<ResidenceTarget_AsNetworkObject_Should>>();
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperId()
    {
        var residence = GetRandomResidence(_logger);
        var actual = residence.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(residence.Id, actual.Id);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperName()
    {
        var residence = GetRandomResidence(_logger);
        var actual = residence.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(residence.Name, actual.Name);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperDescription()
    {
        var residence = GetRandomResidence(_logger);
        var actual = residence.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(residence.Description, actual.Content);
        Assert.Equal(residence.MediaType, actual.MediaType?.ToString());
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperSummary()
    {
        var residence = GetRandomResidence(_logger);
        var actual = residence.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(residence.Summary, actual.Summary);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperAddress()
    {
        var residence = GetRandomResidence(_logger);
        var actual = residence.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(residence.Address, actual.AdditionalProperties["schema:Address"]);
    }

    [Fact]
    public void RoundTripSerializationSuccessfully()
    {
        var entity = GetRandomResidence(_logger);
        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Id, deserialized?.Id);
    }


    private static Targets.Residence GetRandomResidence(ILogger logger)
    {
        var uid = Guid.NewGuid();
        var url = new Uri($"https://example.com/residence/{uid}");
        var streetAddress = string.Empty.GetRandomUSAddress();
        var (city, state) = TestHelpers.CityPairs.GetRandom();
        var zip = $"{99.GetRandom(10):00}{999.GetRandom():000}";
        var address = $"{streetAddress}, {city} {state} {zip}";

        var residence = new Targets.Residence()
        {
            Id = url,
            Name = $"{streetAddress}, {city} {state}",
            Address = address,
            Description = $"A _residential_ property located at [{streetAddress} in {city} {state}]({url})",
            MediaType = "text/markdown",
            Summary = $"A potentially AI generated summary describing the property and circumstances of the residence at {streetAddress}",
        };

        logger.LogTrace("Residence: {@Residence}", residence);
        return residence;
    }

}
