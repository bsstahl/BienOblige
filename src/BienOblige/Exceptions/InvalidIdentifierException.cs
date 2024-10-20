namespace BienOblige.Exceptions;

public class InvalidIdentifierException : Exception
{
    const string _errorMessage = "An identifier must be a valid, absolute URI.";

    public InvalidIdentifierException(string uri, string? message) 
        : base(message)
    { }

    internal static void ThrowIfInvalid(string uri)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out var _))
            throw new InvalidIdentifierException(uri, _errorMessage);
    }
}
