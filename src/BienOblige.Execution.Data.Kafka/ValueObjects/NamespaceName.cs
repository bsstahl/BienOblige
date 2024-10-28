using ValueOf;

namespace BienOblige.Execution.Data.Kafka.ValueObjects;

public class NamespaceName : ValueOf<string, NamespaceName>
{
    protected override void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
