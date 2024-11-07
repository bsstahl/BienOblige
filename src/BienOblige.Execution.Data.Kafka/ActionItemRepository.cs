using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActionItemRepository : ICreateActionItems, IUpdateActionItems
    {
        const string topicName = "execution_command_private";

        ILogger _logger;
        IProducer<string, string> _producer;

        public ActionItemRepository(ILogger<ActionItemRepository> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(item);
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(correlationId);

            var value = new Messages.Create(correlationId, DateTimeOffset.UtcNow, 
                item,
                "https://example.com/users/12341234", "Group");

            var message = new Message<string, string>()
            {
                Key = item.Id.Value.ToString(),
                Value = JsonSerializer.Serialize(value)
            };

            var result = await _producer.ProduceAsync(topicName, message);
            _logger.LogInformation("ActionItemRepository.Create: {0}", result);

            return item.Id;
        }

        public async Task<NetworkIdentity> Update(ActionItem changes, NetworkIdentity userId, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(changes);
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(correlationId);
            ArgumentNullException.ThrowIfNull(changes.Id);

            var message = new Message<string, string>()
            {
                Key = changes.Id.Value.ToString(),
                Value = JsonSerializer.Serialize(changes)
            };

            var result = await _producer.ProduceAsync(topicName, message);
            _logger.LogInformation("ActionItemRepository.Create: {0}", result);

            return changes.Id;
        }
    }
}
