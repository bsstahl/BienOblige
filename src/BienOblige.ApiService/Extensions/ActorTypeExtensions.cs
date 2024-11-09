using BienOblige.Execution.Enumerations;

namespace BienOblige.ApiService.Extensions;

public static class ActorTypeExtensions
{
    public static ActorType AsActorType(this string value)
    {
        // TODO: Decide if I want to convert to Enum.Parse<ActorType>(actorType)
        return value.ToLowerInvariant() switch
        {
            "person" => ActorType.Person,
            "application" => ActorType.Application,
            "service" => ActorType.Service,
            "group" => ActorType.Group,
            "organization" => ActorType.Organization,
            _ => throw new ArgumentException($"Unknown ActorType: {value}")
        };
    }
}
