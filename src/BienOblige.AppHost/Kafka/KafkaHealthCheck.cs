using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BienOblige.AppHost.Kafka;

public class KafkaHealthCheck(IEnumerable<string> topics, string bootStrapServer) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bootStrapServer);

        try
        {
            int attemptsRemaining = 100;
            bool topicCreated = false;
            while (!topicCreated && attemptsRemaining > 0)
            {
                topics.ToList().ForEach(t => topicCreated = CreateTopicsAsync(t, bootStrapServer));
                attemptsRemaining--;
                Task.Delay(100, cancellationToken);
            }

            return topicCreated
                ? Task.FromResult(HealthCheckResult.Healthy())
                : Task.FromResult(HealthCheckResult.Unhealthy());
        }
        catch (Exception ex)
        {
            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
        }
    }

    private static bool CreateTopicsAsync(string topic, string bootstrapServers)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        try
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
            if (metadata.Topics.Any(x => x.Topic == topic))
            {
                return true;
            }
            adminClient.CreateTopicsAsync(new[] { new TopicSpecification { Name = topic, ReplicationFactor = 1, NumPartitions = 1 } }).Wait();
            return true;

        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            return false;
        }
    }
}