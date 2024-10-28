using BienOblige.Execution.Builders;
using BienOblige.Execution.Enumerations;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.Execution.Data.Kafka.Test;

[ExcludeFromCodeCoverage]
public class Create_Ctor_Should
{
    public Create_Ctor_Should(ITestOutputHelper output)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Xunit(output).MinimumLevel.Verbose()
            .CreateLogger();
    }

    [Fact]
    public void ProduceAValidJsonLDCreateMessage()
    {
        var message = new Builders.CreateMessageBuilder()
            .CorrelationId($"urn:uri:{Guid.NewGuid()}")
            .PublishedNow()
            .Actor(new ActorBuilder()
                .Id($"https://example.org/users/{Guid.NewGuid()}")
                .ActorType(ActorType.Person))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Title(string.Empty.GetRandom()))
            .Build();

        // In prod we could use either the Builer above or
        // parameters to the ctor as below. This is a test
        // of the ctor so we use that method.
        var actualMessage = new Messages.Create(message.CorrelationId, message.Published, 
            message.ActionItem.Id, message.ActionItem.Name, 
            message.Actor.Id, message.Actor.Type);

        var actual = message.ToString();
        Log.Logger.Verbose(actual);

        var doc = JsonDocument.Parse(actual);
        var root = doc.RootElement;

        // Assert enough values to verify that the JsonLd
        // document was created correctly
        var expectedContext = new[] {
            "https://www.w3.org/ns/activitystreams",
            "bienoblige:https://bienoblige.com/ns"
        };
        var actualContext = root.GetProperty("@context")
            .EnumerateArray()
            .ToList()
            .Select(t => t.ValueKind.Equals(JsonValueKind.String)
                ? t.GetString() : $"{t.EnumerateObject().First().Name}:{t.EnumerateObject().First().Value}");
        Assert.Equal(expectedContext, actualContext);

        Assert.Equal(message.CorrelationId, root.GetProperty("id").GetString());
        Assert.Equal(message.Published, root.GetProperty("published").GetDateTimeOffset());

        var actualActor = root.GetProperty("actor");
        Assert.Equal(message.Actor.Id, actualActor.GetProperty("id").GetString());
        Assert.Equal(message.Actor.Type, actualActor.GetProperty("type").GetString());

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
