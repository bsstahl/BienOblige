namespace BienOblige.Execution.Data.Kafka.Extensions;

internal static class StringExtensions
{
    internal static BienOblige.ActivityStream.Enumerations.ActorType AsActorType(this string actorType)
    {
        return Enum.Parse<BienOblige.ActivityStream.Enumerations.ActorType>(actorType);
    }
}
