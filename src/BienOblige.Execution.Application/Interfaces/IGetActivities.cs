using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IGetActivities: IDisposable
{
    Task<IManageTransactions<Activity>?> GetActivity(CancellationToken stoppingToken);
}
