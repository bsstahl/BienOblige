using BienOblige.Demand.Aggregates;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Interfaces;

public interface ICreateActionItems
{
    Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId);
}
