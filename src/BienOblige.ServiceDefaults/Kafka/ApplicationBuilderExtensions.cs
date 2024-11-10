using BienOblige.Execution.Data.Kafka.Constants;

namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(
        this IDistributedApplicationBuilder builder,
        string serviceName)
    {
        return builder
            .AddKafka(serviceName)
            .WithHealthCheck([
                Topics.CommandChannelName,
                Topics.ActionItemsPublicChannelName,
                Topics.CompliancePublicChannelName
            ])
            .WithKafkaUI();
    }
}
