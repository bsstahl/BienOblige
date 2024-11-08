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

        //public async Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity creatorId, string creatorType, string correlationId)
        //{
        //    return (await Create(new ActionItem[] { item }, creatorId, creatorType, correlationId)).Single();
        //}

        public async Task<IEnumerable<NetworkIdentity>> Create(IEnumerable<ActionItem> items, NetworkIdentity creatorId, string creatorType, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(creatorId);
            ArgumentNullException.ThrowIfNull(correlationId);

            if (items.Count() == 0)
                throw new ArgumentException("No ActionItems to create");

            var results = new List<NetworkIdentity>();
            foreach (var item in items)
            {
                var value = new Messages.Create(correlationId, DateTimeOffset.UtcNow,
                    item, creatorId.Value.ToString(), creatorType);

                var message = new Message<string, string>()
                {
                    Key = item.Id.Value.ToString(),
                    Value = JsonSerializer.Serialize(value)
                };

                var result = await _producer.ProduceAsync(topicName, message);

                // TODO: Validate that the result is a successful publication

                results.Add(item.Id);
            }

            return results;
        }

        public async Task<NetworkIdentity> Update(ActionItem changes, NetworkIdentity updaterId, string updaterType, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(changes);
            ArgumentNullException.ThrowIfNull(updaterId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(updaterType);
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
