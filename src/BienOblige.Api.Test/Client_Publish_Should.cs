using BienOblige.Api.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class Client_Publish_Should
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;

    public Client_Publish_Should(ITestOutputHelper output)
    {
        var config = new ConfigurationBuilder()
            .Build();

        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<System.Net.Http.HttpClient, Mocks.HttpClient>()
            .AddSingleton<ApiClient.Activities>(s => new ApiClient.Activities(
                s.GetRequiredService<ILogger<Client_Publish_Should>>(),
                s.GetRequiredService<IConfiguration>(),
                s.GetRequiredService<System.Net.Http.HttpClient>()))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<Client_Publish_Should>>();
        _config = _services.GetRequiredService<IConfiguration>();
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
        var httpClient = _services.GetRequiredService<System.Net.Http.HttpClient>() as Mocks.HttpClient ?? throw new InvalidOperationException("No Mock HttpClient found");
        _logger.LogInformation("Activity Request: \r\n\r\n{@Activities}", httpClient.JsonRequestMessages);

        // Assert
        var actual = httpClient.ActivityRequests;
        Assert.NotNull(actual);
        Assert.Equal(4, actual.Count());
    }


}
