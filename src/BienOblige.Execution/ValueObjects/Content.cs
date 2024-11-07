using ValueOf;

namespace BienOblige.Execution.ValueObjects;

public class Content : ValueOf<string, Content>
{
    override protected void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
