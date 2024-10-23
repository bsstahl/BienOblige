using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;
using BienOblige.Execution.Exceptions;

namespace BienOblige.Execution.Application;

public class Client
{
    private readonly ILogger _logger;
    // private readonly IGetActionItems _actionItemReader;
    private readonly ICreateActionItems _actionItemCreator;

    public Client(ILogger<Client> logger, ICreateActionItems actionItemCreator)
    {
        _logger = logger;
        // _actionItemReader = actionItemReader;
        _actionItemCreator = actionItemCreator;
    }

    public async Task<NetworkIdentity> CreateActionItem(ActionItem item, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(userId);

        //if (await _actionItemReader.Exists(item.Id))
        //{
        //    _logger.LogError("ActionItem with ID {ActionItemId} already exists", item.Id);
        //    throw new DuplicateIdentifierException(item.Id);
        //}
        //else
        return await _actionItemCreator.Create(item, userId, correlationId);
    }

    public Task CancelActionItem(string id, string userId, string correlationId)
    {
        throw new NotImplementedException();
    }

    public Task AssignExecutor(NetworkIdentity actionItemId, NetworkIdentity executorId, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(actionItemId);
        ArgumentNullException.ThrowIfNull(executorId);
        ArgumentNullException.ThrowIfNull(userId);

        throw new NotImplementedException();
    }
}
