using BienOblige.ValueObjects;
using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Application.Test.Extensions;
using Microsoft.Extensions.Logging;

namespace BienOblige.Demand.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemCreator : ICreateActionItems
{
    private readonly ILogger _logger;
    private readonly Mock<ICreateActionItems> _actionItemsCreator;
    private readonly IServiceProvider _services;

    public MockActionItemCreator(ILogger<MockActionItemCreator> logger, IServiceProvider services, IEnumerable<ActionItem> itemsToCreate)
    {
        _logger = logger;
        _actionItemsCreator = new();
        _services = services;

        var correlationId = Guid.NewGuid().ToString();
        itemsToCreate.ToList()
            .ForEach(i => this
                .SetupCreateActionItem(i, (null as NetworkIdentity).CreateRandom(), correlationId));
    }

    public async Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId)
    {
        return await _actionItemsCreator.Object.Create(item, userId, correlationId);
    }

    internal MockActionItemCreator SetupCreateActionItem(ActionItem item, NetworkIdentity userId, string correlationId)
    {
        var itemId = item.Id.Value.ToString();
        var uId = userId.Value.ToString();
        var cId = correlationId;

        _actionItemsCreator
            .Setup(x => x.Create(It.IsAny<ActionItem>(), It.IsAny<NetworkIdentity>(), It.IsAny<string>()))
            .Returns(Task.FromResult(item.Id))
            .Callback<ActionItem, NetworkIdentity, string>((p_item, p_userId, p_correlationId) =>
                {
                    if (!itemId.Equals(p_item.Id.Value.ToString()))
                        throw new ArgumentException($"Incorrect Item Id: Expected {itemId} but got {p_item.Id.Value.ToString()}");

                    if (!uId.Equals(p_userId.Value.ToString()))
                        throw new ArgumentException($"Incorrect User Id: Expected {userId} but got {p_userId.Value.ToString()}");

                    if (!cId.Equals(p_correlationId))
                        throw new ArgumentException($"Incorrect Correlation Id: Expected {correlationId} but got {p_correlationId}");
                })
            .Verifiable(Times.Once);
        
        return this;
    }

    public void VerifyAll()
    {
        _actionItemsCreator.VerifyAll();
    }
}
