using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface ICreateActionItems
{
    // Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity creatorId, string creatorType, string correlationId);
    Task<IEnumerable<NetworkIdentity>> Create(IEnumerable<ActionItem> items, NetworkIdentity creatorId, string creatorType, string correlationId);
}
