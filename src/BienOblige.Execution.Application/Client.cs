using BienOblige.ValueObjects;

namespace BienOblige.Execution.Application;

public class Client
{
    public Task AssignExecutor(NetworkIdentity actionItemId, NetworkIdentity executorId, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(actionItemId);
        ArgumentNullException.ThrowIfNull(executorId);

        throw new NotImplementedException();
    }
}
