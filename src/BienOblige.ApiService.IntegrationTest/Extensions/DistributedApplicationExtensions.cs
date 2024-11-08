using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BienOblige.ApiService.IntegrationTest.Extensions;

internal static class DistributedApplicationExtensions
{
    internal static (ILogger, HttpClient) GetRequiredServices(
        this DistributedApplication? app, 
        Guid correlationId, 
        Guid userId)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resources = applicationModel.Resources;
        var resource = resources
            .SingleOrDefault(r => string.Equals(r.Name, "api", StringComparison.OrdinalIgnoreCase)) as IResourceWithEndpoints;

        var httpClient = app.CreateHttpClient("api");
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

        httpClient.DefaultRequestHeaders.Add("x-user-id", $"https://example.org/{userId}");
        httpClient.DefaultRequestHeaders.Add("x-correlation-id", correlationId.ToString());

        var logger = app.Services.GetRequiredService<ILogger<Execution_Create_Should>>();

        logger.LogInformation("Logger and HTTP Client created");

        return (logger, httpClient);
    }

    //internal static IConsumer<string, string> GetKafkaConsumer(this DistributedApplication? app)
    //{
    //    ArgumentNullException.ThrowIfNull(app, nameof(app));
    //    return app.Services.GetRequiredService<IConsumer<string, string>>();
    //}

    //internal static IEnumerable<Object> GetAllMessages(this DistributedApplication? app, string topicName)
    //{
    //    var consumer = app.GetKafkaConsumer();
    //    consumer.Subscribe(topicName);

    //    var messages = new List<ConsumeResult<string, string>>();
    //    try
    //    {
    //        while (true)
    //        {
    //            try
    //            {
    //                // Poll for new messages
    //                var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(10));
    //                if (consumeResult is not null)
    //                {
    //                    messages.Add(consumeResult);
    //                    Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
    //                }
    //            }
    //            catch (ConsumeException e)
    //            {
    //                Console.WriteLine($"Consume error: {e.Error.Reason}");
    //            }
    //        }
    //    }
    //    catch (OperationCanceledException)
    //    {
    //        // Ensure the consumer leaves the group cleanly
    //        consumer.Close();
    //    }

    //    return messages;
    //}
}
