using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Aggregates;

public class NetworkException : NetworkObject
{
    // TODO: Fix the TypeName so it includes an array that has the object type included
    // TODO: Add more properties as needed

    public NetworkException(Content content)
        : base(NetworkIdentity.New(), TypeName.From(typeof(NetworkException)))
    {
        base.Content = content;
    }
}
