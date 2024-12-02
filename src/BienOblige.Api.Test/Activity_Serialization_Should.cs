using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class Activity_Serialization_Should
{
    [Fact]
    public void RoundTripWithTheSameValues()
    {
        // TODO: Add content
        var content = new Api.Builders.ActivitiesCollectionBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Api.Enumerations.ActivityType.Create)
            .Actor(new Api.Builders.ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Api.Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(new Api.Builders.ActionItemCollectionBuilder()
                .Add(new Api.Builders.ActionItemBuilder()
                    .Id(Guid.NewGuid())
                    .Name(string.Empty.GetRandom())
                    .Content(string.Empty.GetRandom(), "text/plain")))
            .Build().Single();

        var serialized = JsonSerializer.Serialize(content);
        var deserialized = JsonSerializer.Deserialize<Api.Entities.Activity>(serialized);

        Assert.NotNull(deserialized);
        Assert.Equal(content.CorrelationId, deserialized.CorrelationId);
    }
}
