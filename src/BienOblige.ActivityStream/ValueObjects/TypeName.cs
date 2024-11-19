using BienOblige.ActivityStream.Enumerations;
using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class TypeName : ValueOf<string, TypeName>
{ 
    public static TypeName From(Type value) 
        => TypeName.From(value.Name);

    public static TypeName From(Enumerations.ObjectType value)
        => TypeName.From(value.ToString());

    public static TypeName From(ActorType value)
        => TypeName.From(value.ToString());

    public static TypeName From(ActivityType value)
        => TypeName.From(value.ToString());
}
