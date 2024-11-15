using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IGetActionItems
{
    Task<bool> Exists(NetworkIdentity id);
    Task<ActionItem?> Get(NetworkIdentity id);
}
