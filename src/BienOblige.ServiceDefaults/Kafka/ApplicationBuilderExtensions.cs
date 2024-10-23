namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(this IDistributedApplicationBuilder builder)
    {
        return builder
            .AddKafka("kafka")
            .WithHealthCheck([
                "execution_command_private",
                "execution_actionitems_public",
                "execution_compliance_public"
            ])
            .WithKafkaUI();
    }

}
