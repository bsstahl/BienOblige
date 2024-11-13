using ValueOf;

namespace BienOblige.Execution.Data.Kafka.Messages;

public class NamespaceName : ValueOf<string, NamespaceName>
{
    protected override void Validate()
    {
        ArgumentException
            .ThrowIfNullOrWhiteSpace(Value, nameof(Value));
    }
}
