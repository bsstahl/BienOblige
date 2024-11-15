using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;
using BienOblige.Execution.Application.Enumerations;
using BienOblige.ActivityStream.Aggregates;

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

        foreach (var item in items)
        {
            item.Generator ??= updatingActor;
            item.LastUpdatedBy = updatingActor;
            item.LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        return items.Any()
            ? await _activityCreator.Create(ActivityType.Create, items, updatingActor, correlationId)
            : throw new ArgumentException("No ActionItems to create");
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
