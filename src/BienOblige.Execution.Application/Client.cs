using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;
using BienOblige.Execution.Application.Enumerations;

namespace BienOblige.Execution.Application;

public class Client
{
    private readonly ILogger _logger;
    private readonly ICreateActivities _activityCreator;

    public Client(ILogger<Client> logger, ICreateActivities activityCreator)
    {
        _logger = logger;
        _activityCreator = activityCreator;
    }

    public async Task<IEnumerable<NetworkIdentity>> CreateActionItem(IEnumerable<ActionItem> items, 
        Actor updatingActor, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(updatingActor);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(correlationId);

        if (items.Count() == 0)
            throw new ArgumentException("No ActionItems to create");

        // TODO: Figure out how to manage the correlation Id so that it doesn't have to get passed-in to the model layers
        // since it is not needed for the Application proper (just the wire formats)

        //if (await _actionItemReader.Exists(item.Id))
        //{
        //    _logger.LogError("ActionItem with ID {ActionItemId} already exists", item.Id);
        //    throw new DuplicateIdentifierException(item.Id);
        //}
        //else

        return await _activityCreator.Create(ActivityType.Create, items, updatingActor, correlationId);
    }

    public async Task<NetworkIdentity> UpdateActionItem(ActionItem changes, Actor updatingActor, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(changes);
        ArgumentNullException.ThrowIfNull(updatingActor);
        ArgumentNullException.ThrowIfNull(changes.Id);

        var result = await _activityCreator.Create(ActivityType.Update, [ changes ], updatingActor, correlationId);
        return result.Single();
    }

    public Task AssignExecutor(NetworkIdentity actionItemId, NetworkIdentity executorId, Actor assigningActor, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(actionItemId);
        ArgumentNullException.ThrowIfNull(executorId);
        ArgumentNullException.ThrowIfNull(assigningActor);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(correlationId);

        throw new NotImplementedException();
    }
}
