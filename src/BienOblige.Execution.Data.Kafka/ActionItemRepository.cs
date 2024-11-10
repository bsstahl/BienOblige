using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Kafka.Constants;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActionItemRepository : ICreateActionItems, IUpdateActionItems, IGetActionItems
    {
        ILogger _logger;
        IProducer<string, string> _producer;

        public ActionItemRepository(ILogger<ActionItemRepository> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<IEnumerable<NetworkIdentity>> Create(IEnumerable<ActionItem> items, 
            Actor actor, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(actor);
            ArgumentNullException.ThrowIfNull(correlationId);

            if (items.Count() == 0)
                throw new ArgumentException("No ActionItems to create");

            var results = new List<NetworkIdentity>();
            foreach (var item in items)
            {
                var value = new Messages.Create(correlationId, DateTimeOffset.UtcNow,
                    item, actor.Id.Value.ToString(), actor.Type.ToString());

                var message = new Message<string, string>()
                {
                    Key = item.Id.Value.ToString(),
                    Value = JsonSerializer.Serialize(value)
                };

                var result = await _producer.ProduceAsync(Topics.CommandChannelName, message);

                // TODO: Validate that the result is a successful publication

                results.Add(item.Id);
            }

            return results;
        }

        public Task<bool> Exists(NetworkIdentity id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionItem?> Get(NetworkIdentity id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ActionItem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<NetworkIdentity> Update(ActionItem changes, 
            Actor actor, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(changes);
            ArgumentNullException.ThrowIfNull(actor);
            ArgumentNullException.ThrowIfNull(correlationId);
            ArgumentNullException.ThrowIfNull(changes.Id);

            var message = new Message<string, string>()
            {
                Key = changes.Id.Value.ToString(),
                Value = JsonSerializer.Serialize(changes)
            };

            var result = await _producer.ProduceAsync(Topics.CommandChannelName, message);
            _logger.LogInformation("ActionItemRepository.Create: {0}", result);

            return changes.Id;
        }
    }
}
