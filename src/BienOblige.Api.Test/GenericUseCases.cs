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
public class GenericUseCases
{
    const string baseUrl = "https://metrotransit.com";

    private NetworkIdentity complianceStudioServiceId = NetworkIdentity.From(baseUrl, "service", "compliance-studio");

    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public GenericUseCases(ITestOutputHelper output)
    {
        _output = output;
        _config = new ConfigurationBuilder()
            .Build();
        _services = new ServiceCollection()
            .UseTestServices<Activities_PublishSinglular_Should>(_config, _output)
            .BuildServiceProvider();
    }

    [Fact]
    public async Task SimplestPossibleActionItem()
    {
        // Arrange
        var activity = new CreateActionItemActivityBuilder()
            .Actor(new ActorBuilder()
                .Id("https://example.com/services/example-service-1")
                .ActorType(Enumerations.ActorType.Service))
            .ActionItem(new ActionItemBuilder()
                .Name("The Simplest Possible Action Item")
                .Content("This is the content of the simplest possible Action Item", "text/plain"))
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

}
