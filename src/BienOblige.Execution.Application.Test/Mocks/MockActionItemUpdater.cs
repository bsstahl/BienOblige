using BienOblige.Execution.Application.Interfaces;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemUpdater : IUpdateActionItems
{
    public Task<NetworkIdentity> Update(NetworkObject changes, Actor updatingActor, string correlationId)
    {
        throw new NotImplementedException();
    }
}
