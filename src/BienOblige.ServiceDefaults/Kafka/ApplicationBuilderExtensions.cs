using BienOblige.Execution.Data.Kafka.Constants;

namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(
        this IDistributedApplicationBuilder builder, 
        string kafkaServiceName)
    {
        return builder
            .AddKafka(kafkaServiceName)
            .WithHealthCheck([
                Topics.CommandChannelName,
                Topics.ActionItemsPublicChannelName,
                Topics.CompliancePublicChannelName
            ])
            .WithKafkaUI();
    }

}
