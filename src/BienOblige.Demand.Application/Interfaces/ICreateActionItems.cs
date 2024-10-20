using BienOblige.Demand.Aggregates;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Interfaces;

public interface ICreateActionItems
{
    NetworkIdentity Create(ActionItem item, NetworkIdentity userId);
}
