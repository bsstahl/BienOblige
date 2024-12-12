using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Kafka.Constants;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Data.Kafka.Extensions;

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

            // Use the Target Object's Id as the key for maintaining order
            // If there is no Target, use the Activity's Object's Id
            // If there is no Activity Object, use the Activity Id
            var activityChildObject = activity.Object; // Usually an ActionItem
            var targetObject = activityChildObject?.GetTarget();
            var messageKey = targetObject?.Id.Value?.ToString()
                ?? activityChildObject?.Id.Value.ToString()
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
