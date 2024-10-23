using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActionItemRepository : ICreateActionItems
    {
        ILogger _logger;
        IProducer<string, string> _producer;

        public ActionItemRepository(ILogger<ActionItemRepository> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId)
        {
            var value = new Messages.Create(correlationId, DateTimeOffset.UtcNow, item.Id.Value.ToString(), "ActionItem Name",
                "ActionItem Content", "Vehicle", "https://example.com/vehicles/1C6RD6KT4CS332867",
                "2022 Dodge Ram 1500", "2022 Dodge Ram pickup with vin 1C6RD6KT4CS332867", "https://example.com/users/12341234");

            var message = new Message<string, string>()
            {
                Key = item.Id.Value.ToString(),
                Value = JsonSerializer.Serialize(value)
            };

            var result = await _producer.ProduceAsync("execution_command_private", message);
            _logger.LogInformation("ActionItemRepository.Create: {0}", result);

            return item.Id;
        }
    }
}
