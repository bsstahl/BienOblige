using BienOblige.Execution.Aggregates;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Application.Interfaces;

public interface IUpdateActionItems
{
    Task<NetworkIdentity> Update(ActionItem changes, NetworkIdentity creatorId, string creatorType, string correlationId);
}
