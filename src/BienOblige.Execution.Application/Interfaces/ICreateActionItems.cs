using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface ICreateActionItems
{
    Task<IEnumerable<NetworkIdentity>> Create(IEnumerable<ActionItem> items, Actor actor, string correlationId);
}
