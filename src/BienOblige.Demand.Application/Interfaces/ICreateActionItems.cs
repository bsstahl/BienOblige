using BienOblige.ValueObjects;
using BienOblige.Demand.Aggregates;

namespace BienOblige.Demand.Application.Interfaces;

public interface ICreateActionItems
{
    Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId);
}
