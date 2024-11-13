using BienOblige.Execution.Application.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IGetActivities: IDisposable
{
    Task<IManageTransactions<Activity>?> GetActivity(CancellationToken stoppingToken);
}
