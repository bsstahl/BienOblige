using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface ICreateActionItems
{
    Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId);
}
