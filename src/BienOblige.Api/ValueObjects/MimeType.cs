using System.Text.RegularExpressions;
using ValueOf;

namespace BienOblige.Api.ValueObjects;

public class MimeType : ValueOf<string, MimeType>
{
    const string mimePattern = @"^[a-zA-Z0-9!#$&^_.+-]+/[a-zA-Z0-9!#$&^_.+-]+$"; // Regular expression to match a valid MIME type

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(this.Value))
            throw new ArgumentException("MIME type cannot be null or empty.", nameof(MimeType));

        if (!Regex.IsMatch(this.Value, mimePattern))
            throw new ArgumentException($"Invalid MIME type format '{this.Value}'", nameof(MimeType));
    }
}
