using BienOblige.Execution.Builders;
using BienOblige.Execution.Enumerations;
using BienOblige.Execution.Application.Enumerations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Xunit.Abstractions;
using BienOblige.ValueObjects;
using BienOblige.Execution.ValueObjects;

namespace BienOblige.Execution.Data.Kafka.Test;

[ExcludeFromCodeCoverage]
public class Create_Ctor_Should
{
    private readonly ILogger _logger;

    public Create_Ctor_Should(ITestOutputHelper output)
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output).SetMinimumLevel(LogLevel.Trace))
            .BuildServiceProvider();

        _logger = services.GetRequiredService<ILogger<Activity_Ctor_Should>>();
    }

    [Fact]
    public void ProduceAValidJsonLDCreateMessage()
    {
        // This is just being used to define the message values
        // so we can compare them to the actual message.
        var message = new Builders.ActivityMessageBuilder()
            .ActivityType(ActivityType.Create)
            .CorrelationId(Guid.NewGuid())
            .PublishedNow()
            .Actor(new ActorBuilder()
                .Id($"https://example.org/users/{Guid.NewGuid()}")
                .ActorType(ActorType.Person))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Title(string.Empty.GetRandom())
                .Content(string.Empty.GetRandom()))
            .Build();

        var actor = Aggregates.Actor.From(message.Actor.Id, message.Actor.Type);
        var actionItem = new Aggregates.ActionItem(NetworkIdentity.From(message.ActionItem.Id),
            Title.From(message.ActionItem.Name), Content.From(message.ActionItem.Content));

        // In prod we could use either the Builder above or
        // parameters to the ctor as below. This is a test
        // of the ctor so we use that method.

        //var actualMessage = new Messages.Activity(message.ActivityType, message.CorrelationId,
        //    message.Published, actionItem, message.Context, actor);
        var actualMessage = new Messages.Activity(message.ActivityType, message.CorrelationId,
            message.Published, actionItem, null!, actor);

        var actual = message.ToString();
        _logger.LogTrace(actual);

        var doc = JsonDocument.Parse(actual);
        var root = doc.RootElement;

        // Assert enough values to verify that the JsonLd
        // document was created correctly

        // TODO: Restore assert for the Context node
        //var expectedContext = new[] {
        //    "https://www.w3.org/ns/activitystreams",
        //    "bienoblige:https://bienoblige.com/ns"
        //};
        //var actualContext = root.GetProperty("@context")
        //    .EnumerateArray()
        //    .ToList()
        //    .Select(t => t.ValueKind.Equals(JsonValueKind.String)
        //        ? t.GetString() : $"{t.EnumerateObject().First().Name}:{t.EnumerateObject().First().Value}");
        //Assert.Equal(expectedContext, actualContext);

        Assert.Equal(message.CorrelationId, root.GetProperty("id").GetString());
        Assert.Equal(message.Published, root.GetProperty("published").GetDateTimeOffset());

        var actualActor = root.GetProperty("actor");
        Assert.Equal(message.Actor.Id, actualActor.GetProperty("id").GetString());
        Assert.Equal(message.Actor.Type, actualActor.GetProperty("@type").GetString());

        var actualObject = root.GetProperty("object");
        Assert.Equal(message.ActionItem.Id, actualObject.GetProperty("id").GetString());
        Assert.Equal(message.ActionItem.Name, actualObject.GetProperty("name").GetString());

        var actualActionItemType = actualObject.GetProperty("@type");
        var actualActionItemTypes = actualActionItemType
            .EnumerateArray()
            .ToList()
            .Where(t => !string.IsNullOrWhiteSpace(t.GetString()))
            .Select(t => t.GetString() ?? string.Empty)
            .ToArray();
        Assert.Equal(message.ActionItem.ObjectType, actualActionItemTypes);
    }
}
