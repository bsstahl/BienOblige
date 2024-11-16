using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Worker.Extensions;

namespace BienOblige.Execution.Worker;

public class ExecutionService : BackgroundService
{
    private readonly ILogger<ExecutionService> _logger;
    private readonly IGetActivities _consumer;
    private readonly IGetActionItems _readRepo;
    private readonly IUpdateActionItems _writeRepo;

    public ExecutionService(
        ILogger<ExecutionService> logger, IGetActivities consumer,
        IGetActionItems readRepo, IUpdateActionItems writeRepo)
    {
        _logger = logger;
        _consumer = consumer;
        _readRepo = readRepo;
        _writeRepo = writeRepo;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var activityManager = await _consumer.GetActivity(stoppingToken);
            if (activityManager is not null)
            {
                _logger.LogInformation("Received message {Id} from {Timestamp}", activityManager.Content.Id.Value, activityManager.MessageTimestamp.ToString("o"));
                try
                {
                    await activityManager.Process(_logger, _readRepo, _writeRepo);
                    await activityManager.Commit();
                    _logger.LogInformation("Processed message {Id}", activityManager.Content.Id.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to process message {Id}. Rolled-back to be tried again. Error: {Error}", activityManager.Content.Id.Value, ex.Message);
                    await Task.Delay(1000); // TODO: Make the retry delay length configurable
                }   
            }
        }
    }
}