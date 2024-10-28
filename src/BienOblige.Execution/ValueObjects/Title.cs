using ValueOf;

namespace BienOblige.Execution.ValueObjects;

public class Title: ValueOf<string, Title>
{
    override protected void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
