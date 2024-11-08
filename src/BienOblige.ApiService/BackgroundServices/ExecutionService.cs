using Confluent.Kafka;

namespace BienOblige.ApiService.BackgroundServices;

public class ExecutionService : BackgroundService
{
    const string topicName = "execution_command_private";

    private readonly ILogger<ExecutionService> _logger;
    private readonly IConsumer<string, string> _consumer;

    public ExecutionService(ILogger<ExecutionService> logger, IConsumer<string, string> consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(topicName);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                    // Process the message here


                    // Commit the offset if required
                    _consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Error occurred: {ex.Error.Reason}");
                }
            }
        }
        finally
        {
            _consumer.Close(); // Ensure the consumer leaves the group cleanly and final offsets are committed.
        }
    }


    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}