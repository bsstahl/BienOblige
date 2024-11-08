using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application;

public class Client
{
    private readonly ILogger _logger;
    private readonly ICreateActionItems _actionItemCreator;
    private readonly IUpdateActionItems _actionItemUpdater;

    public Client(ILogger<Client> logger, 
        ICreateActionItems actionItemCreator,
        IUpdateActionItems actionItemUpdater)
    {
        _logger = logger;
        _actionItemCreator = actionItemCreator;
        _actionItemUpdater = actionItemUpdater;
    }

    public async Task<IEnumerable<NetworkIdentity>> CreateActionItem(IEnumerable<ActionItem> items, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(userId);

        if (items.Count() == 0)
            throw new ArgumentException("No ActionItems to create");

        // TODO: Figure out how to manage the correlation Id ideally so that it doesn't have
        // to get passed-in to the model layers since it is not needed for the
        // Application proper (just the wire formats)

        //if (await _actionItemReader.Exists(item.Id))
        //{
        //    _logger.LogError("ActionItem with ID {ActionItemId} already exists", item.Id);
        //    throw new DuplicateIdentifierException(item.Id);
        //}
        //else

        return await _actionItemCreator.Create(items, userId, "Person", correlationId);
    }

    public async Task<NetworkIdentity> UpdateActionItem(ActionItem changes, NetworkIdentity userId, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(changes);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(changes.Id);

        return await _actionItemUpdater.Update(changes, userId, "Person", correlationId);
    }

    public Task AssignExecutor(NetworkIdentity actionItemId, NetworkIdentity executorId, NetworkIdentity assignerId, string assignerType, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(actionItemId);
        ArgumentNullException.ThrowIfNull(executorId);
        ArgumentNullException.ThrowIfNull(assignerId);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(assignerType);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(correlationId);

        throw new NotImplementedException();
    }
}
