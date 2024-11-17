using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class Name: ValueOf<string, Name>
{
    override protected void Validate()
    {
        ArgumentNullException
            .ThrowIfNullOrWhiteSpace(this.Value, nameof(this.Value));
    }
}
