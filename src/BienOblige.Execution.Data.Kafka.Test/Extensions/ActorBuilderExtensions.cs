using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.Builders;

namespace BienOblige.Execution.Data.Kafka.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class ActorBuilderExtensions
{
    public static ActorBuilder UseRandomValues(this ActorBuilder builder)
    {
        var idValue = Guid.NewGuid().ToString();
        var actorType = Enum.GetValues<ActorType>().GetRandom();

        return builder
            .Id($"https://example.org/{idValue}")
            .Name($"Name of actor with ID={idValue}")
            .ActorType(actorType);
    }
}
