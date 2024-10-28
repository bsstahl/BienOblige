using BienOblige.Execution.Builders;
using BienOblige.Execution.Enumerations;
using Serilog;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.Execution.Data.Kafka.Test;

public class Create_Ctor_Should
{
    public Create_Ctor_Should(ITestOutputHelper output)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Xunit(output).MinimumLevel.Verbose()
            .CreateLogger();
    }

    //[Fact]
    //public void ProduceAValidJsonLDCreateMessage()
    //{
    //    var correlationId = $"urn:uri:{Guid.NewGuid()}";
    //    var published = DateTime.UtcNow;

    //    var actorId = $"https://example.org/users/{Guid.NewGuid()}";

    //    var actionItem = new ActionItemBuilder()
    //        .Id(Guid.NewGuid())
    //        .Build();

    //    var message = new Messages.Create(
    //        correlationId: correlationId,
    //        published: published,
    //        actionItemId: actionItem.Id.Value.ToString(),
    //        actionItemName: "",
    //        actionItemContent: "",
    //        targetType: "ActionItem",
    //        targetId: "",
    //        targetName: "",
    //        targetDescription: "",
    //        actorId: actorId);

    //    var actual = JsonLdProcessor.Parse(message);
    //    Log.Logger.Verbose(actual);

    //    Assert.True(JsonSerializer.Deserialize<dynamic>(actual));
    //}

    [Fact]
    public void ProduceAValidJsonLDCreateMessage()
    {
        var correlationId = $"urn:uri:{Guid.NewGuid()}";

        var message = new Builders.CreateMessageBuilder()
            .CorrelationId(correlationId)
            .PublishedNow()
            .Actor(new ActorBuilder()
                .Id($"https://example.org/users/{Guid.NewGuid()}")
                .ActorType(ActorType.Person))
            .ActionItem(new ActionItemBuilder()
                .Id(Guid.NewGuid())
                .Title(string.Empty.GetRandom()))
            .Build();

        var actualMessage = new Messages.Create(message.CorrelationId, message.Published, 
            message.ActionItem.Id, message.ActionItem.Name, 
            message.Actor.Id, message.Actor.Type);

        var actual = message.ToString();
        Log.Logger.Verbose(actual);

        // TODO: Assert enough values to verify that the JsonLd was created correctly
        // Assert.True(JsonSerializer.Deserialize<dynamic>(actual));
    }
}
