using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Constants;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Kafka.Constants;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BienOblige.Execution.Application.Enumerations;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActivityWriteRepository : ICreateActivities
    {
        ILogger _logger;
        IProducer<string, string> _producer;

        public ActivityWriteRepository(ILogger<ActivityWriteRepository> logger, IProducer<string, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<IEnumerable<NetworkIdentity>> Create(ActivityType activityType, IEnumerable<ActionItem> items, 
            Actor actor, string correlationId)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(actor);
            ArgumentNullException.ThrowIfNull(correlationId);

            if (items.Count() == 0)
                throw new ArgumentException("No ActionItems to create");

            var context = new List<Messages.Context>()
            { 
               new Messages.Context(Namespaces.RootNamespaceName),
               new Messages.Context(Namespaces.BienObligeNamespaceName, Namespaces.BienObligeNamespaceKey),
               new Messages.Context(Namespaces.SchemaNamespaceName, Namespaces.SchemaNamespaceKey)
            };

            var results = new List<NetworkIdentity>();
            foreach (var item in items)
            {
                var value = new Messages.Activity(ActivityType.Create.ToString(), correlationId, 
                    DateTimeOffset.UtcNow, item, context, actor);

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

    }
}
