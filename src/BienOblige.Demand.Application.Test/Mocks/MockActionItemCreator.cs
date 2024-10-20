using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Application.Test.Extensions;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemCreator : ICreateActionItems
{
    private readonly Mock<ICreateActionItems> _actionItemsCreator;
    private readonly IServiceProvider _services;

    public MockActionItemCreator(IServiceProvider services, IEnumerable<ActionItem> itemsToCreate)
    {
        _actionItemsCreator = new();
        _services = services;

        itemsToCreate.ToList()
            .ForEach(i => this
                .SetupCreateActionItem(i, (null as NetworkIdentity).CreateRandom()));
    }

    public NetworkIdentity Create(ActionItem item, NetworkIdentity userId)
    {
        return _actionItemsCreator.Object.Create(item, userId);
    }

    internal MockActionItemCreator SetupCreateActionItem(ActionItem item, NetworkIdentity userId)
    {
        var itemId = item.Id.Value.ToString();
        var uId = userId.Value.ToString();

        _actionItemsCreator
            .Setup(x => x.Create(It.IsAny<ActionItem>(), It.IsAny<NetworkIdentity>()))
            .Returns(item.Id)
            .Callback<ActionItem, NetworkIdentity>((p_item, p_userId) =>
                {
                    if (!itemId.Equals(p_item.Id.Value.ToString()))
                        throw new ArgumentException($"Incorrect Item Id: Expected {itemId} but got {p_item.Id.Value.ToString()}");

                    if (!uId.Equals(p_userId.Value.ToString()))
                        throw new ArgumentException($"Incorrect User Id: Expected {userId} but got {p_userId.Value.ToString()}");
                })
            .Verifiable(Times.Once);
        
        return this;
    }

    public void VerifyAll()
    {
        _actionItemsCreator.VerifyAll();
    }
}
