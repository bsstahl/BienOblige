using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Aggregates;

public class Activity : NetworkObject
{
    public required ActivityType ActivityType { get; set; }
    public required Actor Actor { get; set; }
    public required ActionItem ActionItem { get; set; }
    public required NetworkIdentity CorrelationId { get; set; }

    public Activity()
    {
        this.ObjectTypeName = GetObjectTypeName();
    }

    public static IEnumerable<TypeName> GetObjectTypeName() => [TypeName.From(nameof(Activity))];
}
