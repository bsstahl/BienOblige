using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Kafka.Constants;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActivityWriteRepository : IPublishActivityCommands
    {
        ILogger _logger;
        IProducer<string, string> _producer;

        public ActivityWriteRepository(ILogger<ActivityWriteRepository> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<NetworkIdentity> Publish(Activity activity)
        {
            ArgumentNullException.ThrowIfNull(activity);

            var publishActivity = Messages.Activity.From(activity);

            // Use the ActionItem Object's Id as the key for maintaining order
            // If there is no Object, use the ActionItem Id
            // If there is no ActionItem, use the Activity Id
            var actionItem = activity.Object as ActionItem;
            var messageKey = actionItem?.Target?.Id.Value?.ToString()
                ?? actionItem?.Id.Value.ToString()
                ?? activity.Id.Value.ToString();

            var message = new Message<string, string>()
            {
                Key = messageKey,
                Value = JsonSerializer.Serialize(publishActivity)
            };

            var result = await _producer.ProduceAsync(Topics.CommandChannelName, message);
            
            // TODO: Add error handling
            
            return activity.Object.Id;
        }
    }
}
