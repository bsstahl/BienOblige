using ValueOf;

namespace BienOblige.Execution.Data.Kafka.ValueObjects;

public class NamespaceKey : ValueOf<string, NamespaceKey>
{
    protected override void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
