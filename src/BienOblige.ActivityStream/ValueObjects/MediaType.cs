using System.Text.RegularExpressions;
using ValueOf;

namespace BienOblige.ActivityStream.ValueObjects;

public class MediaType : ValueOf<string, MediaType>
{
    const string pattern = @"^[a-zA-Z0-9!#$&^_+-]{1,127}/[a-zA-Z0-9!#$&^_+-]{1,127}$";

    protected override void Validate()
    {
        if (!Regex.IsMatch(this.Value, pattern))
            throw new ArgumentException($"Invalid {nameof(MediaType)}: {this.Value}");
    }
}
