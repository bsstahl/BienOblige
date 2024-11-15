namespace BienOblige.ActivityStream.Exceptions;

public class InvalidIdentifierException : Exception
{
    const string _errorMessageTemplate = "An identifier must be a valid, absolute URI. Actual: {0}";

    public InvalidIdentifierException(string uri, string? message) 
        : base(message)
    { }

    internal static void ThrowIfInvalid(string uri)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out var _))
        {
            string message = string.Format(_errorMessageTemplate, uri);
            throw new InvalidIdentifierException(uri, message);
        }
    }
}
