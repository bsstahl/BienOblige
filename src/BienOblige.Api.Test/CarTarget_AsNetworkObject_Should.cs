using BienOblige.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class CarTarget_AsNetworkObject_Should
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public CarTarget_AsNetworkObject_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<CarTarget_AsNetworkObject_Should>>();
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperId()
    {
        var car = GetRandomCar(_logger);
        var actual = car.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(car.Id, actual.Id);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperName()
    {
        var car = GetRandomCar(_logger);
        var actual = car.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(car.Name, actual.Name);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperDescription()
    {
        var car = GetRandomCar(_logger);
        var actual = car.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(car.Description, actual.Content);
        Assert.Equal(car.MediaType, actual.MediaType?.ToString());
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperSummary()
    {
        var car = GetRandomCar(_logger);
        var actual = car.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(car.Summary, actual.Summary);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperVIN()
    {
        var car = GetRandomCar(_logger);
        var actual = car.AsNetworkObject();

        _logger.LogTrace("NetworkObject: {@Object}", actual);
        Assert.Equal(car.Vin, actual.AdditionalProperties["schema:vehicleIdentificationNumber"]);
    }

    [Fact]
    public void RoundTripSerializationSuccessfully()
    {
        var entity = GetRandomCar(_logger);
        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Id, deserialized?.Id);
    }


    private static Targets.Car GetRandomCar(ILogger logger)
    {
        var vin = string.Empty.GetRandomVIN();
        var car = new Targets.Car()
        {
            Id = new Uri($"https://example.com/car/{vin}"),
            Name = $"Car with VIN {vin}",
            Description = "TODO: Add a car description here",
            MediaType = "text/plain",
            Summary = $"TODO: Add a summary of Car {vin}",
            Vin = vin
        };

        logger.LogTrace("Car: {@Car}", car);
        return car;
    }

}
