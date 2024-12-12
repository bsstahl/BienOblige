using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Application.Interfaces;

public interface IUpdateActionItems
{
    Task<NetworkIdentity> Update(NetworkObject changes, Actor actor, string correlationId);
}
