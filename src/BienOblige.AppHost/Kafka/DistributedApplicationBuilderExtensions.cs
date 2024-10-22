namespace BienOblige.AppHost.Kafka;

public static class DistributedApplicationBuilderExtensions
{

    public static IResourceBuilder<KafkaServerResource> WithHealthCheck(this IResourceBuilder<KafkaServerResource> builder, IEnumerable<string> topics)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new KafkaHealthCheck(topics, cs)));
    }

}
