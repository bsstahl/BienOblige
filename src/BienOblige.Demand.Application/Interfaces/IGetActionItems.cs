using BienOblige.Demand.Aggregates;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Interfaces;

public interface IGetActionItems
{
    bool Exists(NetworkIdentity id);
    ActionItem Get(NetworkIdentity id);
}
