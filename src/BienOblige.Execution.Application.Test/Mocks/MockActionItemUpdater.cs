using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemUpdater : IUpdateActionItems
{
    public Task<NetworkIdentity> Update(ActionItem changes, NetworkIdentity userId, string correlationId)
    {
        throw new NotImplementedException();
    }
}
