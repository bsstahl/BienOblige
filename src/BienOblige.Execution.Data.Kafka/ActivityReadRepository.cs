using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application;
using BienOblige.Execution.Data.Kafka.Constants;
using BienOblige.Execution.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka;

public class ActivityReadRepository : IGetActivities
{
    ILogger _logger;
    IConsumer<string, string> _consumer;

    private bool disposedValue;

    public ActivityReadRepository(ILogger<ActivityReadRepository> logger, IConsumer<string, string> consumer)
    {
        _logger = logger;
        _consumer = consumer;
        _consumer.Subscribe(Topics.CommandChannelName);
    }

    public async Task<IManageTransactions<Activity>?> GetActivity(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            var messageValue = consumeResult.Message.Value;

            _logger.LogInformation($"Consumed message '{messageValue}' at: '{consumeResult.TopicPartitionOffset}'.");

            var content = JsonSerializer.Deserialize<Messages.Activity>(messageValue);

            var headers = consumeResult.Message.Headers.ToDictionary(
                h => h.Key,
                h => Encoding.UTF8.GetString(h.GetValueBytes()));

            var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(consumeResult.Message.Timestamp.UnixTimestampMs);

            var activityAggregate = content?.AsAggregate();
            if (activityAggregate is null)
            {
                _logger.LogError("Failed to deserialize message content");
                throw new InvalidOperationException("Failed to deserialize message content");
            }

            var trx = new ReadTransactionManager<Activity>(_consumer, consumeResult, activityAggregate, headers, timestamp);
            return await Task.FromResult(trx);
        }
        catch (ConsumeException ex)
        {
            _logger.LogError($"Error occurred: {ex.Error.Reason}");
            return (null as IManageTransactions<Activity>);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                _consumer?.Close();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
