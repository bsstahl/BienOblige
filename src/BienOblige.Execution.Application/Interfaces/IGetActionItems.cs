using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IGetActionItems
{
    Task<bool> Exists(NetworkIdentity id);
    Task<ActionItem?> Get(NetworkIdentity id);
}
