using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Exceptions;

public class DuplicateIdentifierException : Exception
{
    const string _errorMessage = "Each ActionItem must have a unique identifier.";

    public NetworkIdentity Id { get; set; }

    public DuplicateIdentifierException(NetworkIdentity id) 
        : base(_errorMessage)
    {
        this.Id = id;
    }
}
