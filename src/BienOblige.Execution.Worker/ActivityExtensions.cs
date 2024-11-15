using BienOblige.Execution.Application.Enumerations;
using BienOblige.Execution.Application.Interfaces;

namespace BienOblige.Execution.Worker;

internal static class ActivityExtensions
{
    internal static async Task Process(
        this IManageTransactions<Application.Aggregates.Activity> activityManager,
        ILogger logger,
        IGetActionItems readRepo,
        IUpdateActionItems writeRepo)
    {
        var activityType = activityManager.Content.ActivityType;

        // TODO: Use the Strategy pattern to avoid this if-else block
        if (activityType == ActivityType.Create)
            await activityManager.ProcessCreate(logger, readRepo, writeRepo);
        else if (activityType == ActivityType.Update)
            await activityManager.ProcessUpdate(logger, readRepo, writeRepo);
        else
        {
            // Since there is nothing we can do with this activity type, we just skip it for now
            // TODO: If there is an existing ActionItem, add an exception to it, otherwise send it to a dead-letter-queue or similar
            logger.LogError("An unsupported activity type {ActivityType} was specified in message Id {Id}. Message skipped.", activityManager.Content.ActivityType, activityManager.Content.Id.Value);
        }
    }

    private static async Task ProcessCreate(this IManageTransactions<Application.Aggregates.Activity> activityManager,
        ILogger logger,
        IGetActionItems readRepo,
        IUpdateActionItems writeRepo)
    {
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["ActivityId"] = activityManager.Content.Id.Value,
            ["MethodName"] = nameof(ProcessCreate)
        });

        var actionItemId = activityManager.Content.ActionItem.Id;
        if (await readRepo.Exists(actionItemId))
        {
            // TODO: Add an exception to the existing ActionItem
            logger.LogInformation("ActionItem {Id} already exists. Message {CorrelationId}", actionItemId, activityManager.Content.Id.Value);
            throw new NotImplementedException();
        }
        else
        {
            var item = await writeRepo.Update(activityManager.Content.ActionItem, activityManager.Content.Actor, activityManager.Content.Id.Value.ToString());
            logger.LogInformation("Created ActionItem {Id} with correlation {CorrelationId}", item.Value.ToString(), activityManager.Content.Id.Value);
        }
    }

    private static Task ProcessUpdate(this IManageTransactions<Application.Aggregates.Activity> activityManager,
        ILogger logger,
        IGetActionItems readRepo,
        IUpdateActionItems writeRepo)
    {
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["ActivityId"] = activityManager.Content.Id.Value,
            ["MethodName"] = nameof(ProcessUpdate)
        });

        throw new NotImplementedException();
    }
}