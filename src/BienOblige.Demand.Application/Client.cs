using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Exceptions;
using BienOblige.Demand.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BienOblige.Demand.Application;

public class Client
{
    private readonly ILogger _logger;
    private readonly IGetActionItems _actionItemReader;
    private readonly ICreateActionItems _actionItemCreator;

    public Client(ILogger<Client> logger, IGetActionItems actionItemReader, ICreateActionItems actionItemCreator)
    {
        _logger = logger;
        _actionItemReader = actionItemReader;
        _actionItemCreator = actionItemCreator;
    }

    public async Task<NetworkIdentity> CreateActionItem(ActionItem item, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(userId);

        if (await _actionItemReader.Exists(item.Id))
        {
            _logger.LogError("ActionItem with ID {ActionItemId} already exists", item.Id);
            throw new DuplicateIdentifierException(item.Id);
        }
        else
            return await _actionItemCreator.Create(item, userId, correlationId);
    }

    public Task CancelActionItem(string id, string userId, string correlationId)
    {
        throw new NotImplementedException();
    }

}
