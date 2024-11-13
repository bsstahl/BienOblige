using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockActionItemReader : IGetActionItems
{
    private readonly Mock<IGetActionItems> _actionItemsReader;
    private readonly ILogger _logger;

    public MockActionItemReader(ILogger<MockActionItemReader> logger)
    {
        _actionItemsReader = new();
        _logger = logger;
    }

    public async Task<bool> Exists(NetworkIdentity id)
        => (await this.Get(id)) is not null;

    public async Task<ActionItem?> Get(NetworkIdentity id)
    {
        _logger.LogInformation("Fetching ActionItem {ActionItemId}", id);
        var result = await _actionItemsReader.Object.Get(id);
        if (result is null)
            _logger.LogWarning("ActionItem {ActionItemId} not found", id);

        return result;
    }

    public Task<IEnumerable<ActionItem>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ActionItem>> GetGraph(NetworkIdentity parentId)
    {
        throw new NotImplementedException();
    }

    internal MockActionItemReader SetupExistingActionItem(ActionItem item)
    {
        _actionItemsReader
            .Setup(x => x.Get(item.Id))
            .Returns(Task.FromResult<ActionItem?>(item));
        return this;
    }

}
