using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.CacheConnector.Entities;
using BienOblige.Execution.Data.Kafka.Constants;
using Confluent.Kafka;
using StackExchange.Redis;

namespace BienOblige.Execution.CacheConnector;

public class ActionItemConnector : BackgroundService
{
    const string subscriptionPattern = "__keyevent@0__:*";

    private readonly ILogger<ActionItemConnector> _logger;
    IConnectionMultiplexer _connectionMux;
    IGetActionItems _readRepository;
    IProducer<string, string> _producer;

    public ActionItemConnector(ILogger<ActionItemConnector> logger, 
        IConnectionMultiplexer connectionMux, 
        IGetActionItems readRepository,
        IProducer<string, string> producer)
    {
        _logger = logger;
        _connectionMux = connectionMux;
        _readRepository = readRepository;
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to all keyspace events for the default database (0)
        var subscriber = _connectionMux.GetSubscriber();
        var channel = new RedisChannel(subscriptionPattern, RedisChannel.PatternMode.Pattern);
        subscriber.Subscribe(channel, async (c, v) => await PublishChange(c, v));

        // wait for the cancellation token to be triggered
        await Task.Run(() => stoppingToken.WaitHandle.WaitOne(), stoppingToken);
    }

    private async Task PublishChange(RedisChannel _, RedisValue changedKey)
    {
        var key = changedKey.ToString();
        var value = await _readRepository.Get(NetworkIdentity.From(key));

        _logger.LogInformation("Update made to ActionItem - {@Key} : {@Value}", key, value);

        if (value is not null)
        {
            // TODO: Determine if we need to handle the case where the value is null

            var message = new Message<string, string>()
            {
                Key = key,
                Value = NetworkObject.From(value)?.ToString() ?? string.Empty
            };

            // TODO: Pass along the Correlation Id so we can trace the message through the system
            var result = await _producer.ProduceAsync(Topics.ActionItemsPublicChannelName, message);
        }

        // TODO: Validate that the result is a successful publication
    }
}
