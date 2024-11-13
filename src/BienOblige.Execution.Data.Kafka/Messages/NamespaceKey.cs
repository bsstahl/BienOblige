using ValueOf;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class NamespaceKey : ValueOf<string, NamespaceKey>
{
    protected override void Validate()
    {
        ArgumentException
            .ThrowIfNullOrWhiteSpace(Value, nameof(Value));
    }
}
