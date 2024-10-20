using BienOblige.Demand.Aggregates;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Interfaces;

public interface IGetActionItems
{
    Task<bool> Exists(NetworkIdentity id);
    Task<ActionItem?> Get(NetworkIdentity id);
}
