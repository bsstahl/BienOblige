using BienOblige.ValueObjects;
using BienOblige.Demand.Aggregates;

namespace BienOblige.Demand.Application.Interfaces;

public interface IGetActionItems
{
    Task<bool> Exists(NetworkIdentity id);
    Task<ActionItem?> Get(NetworkIdentity id);
}
