using BienOblige.Execution.Enumerations;

namespace BienOblige.ApiService.Extensions;

public static class ActorTypeExtensions
{
    public static ActorType AsActorType(this string value)
    {
        return Enum.TryParse<ActorType>(value, out var actorType) 
            ? actorType 
            : throw new ArgumentException($"Unknown ActorType: {value}");
    }
}
