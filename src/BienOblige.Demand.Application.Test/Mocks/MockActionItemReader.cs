using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemReader : IGetActionItems
{
    private readonly Mock<IGetActionItems> _actionItemsReader;
    private readonly IServiceProvider _services;

    public MockActionItemReader(IServiceProvider services)
    {
        _actionItemsReader = new();
        _services = services;
    }

    public bool Exists(NetworkIdentity id)
        => this.Get(id) is not null;

    public ActionItem Get(NetworkIdentity id)
        => _actionItemsReader.Object.Get(id);

    internal MockActionItemReader SetupExistingActionItem(ActionItem item)
    {
        _actionItemsReader
            .Setup(x => x.Get(item.Id))
            .Returns(item);
        return this;
    }

}
