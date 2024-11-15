using BienOblige.Execution.Aggregates;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IUpdateActionItems
{
    Task<NetworkIdentity> Update(ActionItem changes, Actor actor, string correlationId);
}
