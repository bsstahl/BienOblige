using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Exceptions;

public class ActionItemNotFoundException: Exception
{
    public NetworkIdentity Id { get; private set; }

    public ActionItemNotFoundException(NetworkIdentity actionItemId)
        : base($"Action item with id {actionItemId} not found.")
    {
        this.Id = actionItemId;
    }
}
