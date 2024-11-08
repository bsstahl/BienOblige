using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Extensions;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemCreator : ICreateActionItems
{
    private readonly ILogger _logger;
    private readonly Mock<ICreateActionItems> _actionItemsCreator;
    private readonly IServiceProvider _services;

    public MockActionItemCreator(ILogger<MockActionItemCreator> logger, IServiceProvider services) // , IEnumerable<ActionItem> itemsToCreate)
    {
        _logger = logger;
        _actionItemsCreator = new();
        _services = services;

        //var correlationId = Guid.NewGuid().ToString();
        //this.SetupCreateActionItem(itemsToCreate, 
        //    (null as NetworkIdentity).CreateRandom(), 
        //    correlationId);
    }

    public async Task<IEnumerable<NetworkIdentity>> Create(IEnumerable<ActionItem> items, NetworkIdentity creatorId, string creatorType, string correlationId)
    {
        return await _actionItemsCreator.Object.Create(items, creatorId, creatorType, correlationId);
    }

    internal MockActionItemCreator SetupCreateActionItem(IEnumerable<ActionItem> items, NetworkIdentity userId, string correlationId)
    {
        var itemIds = items.Select(i => i.Id);
        var itemIdValues = itemIds.Select(i => i.Value.ToString());
        var uId = userId.Value.ToString();
        var cId = correlationId;

        _actionItemsCreator
            .Setup(x => x.Create(It.IsAny<IEnumerable<ActionItem>>(), It.IsAny<NetworkIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(itemIds))
            .Callback<IEnumerable<ActionItem>, NetworkIdentity, string, string>((p_item, p_userId, p_userType, p_correlationId) =>
                {
                    p_item.ToList().ForEach(p_item => {
                        if (!itemIdValues.Contains(p_item.Id.Value.ToString()))
                            throw new ArgumentException($"Expected Id {p_item.Id.Value.ToString()} not found in {itemIdValues}");
                        });

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
