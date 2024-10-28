using ValueOf;

namespace BienOblige.Execution.ValueObjects;

public class ActorName : ValueOf<string, ActorName>
{
    protected override void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
