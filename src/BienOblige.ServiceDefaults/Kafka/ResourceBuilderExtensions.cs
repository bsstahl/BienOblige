namespace BienOblige.ServiceDefaults.Kafka;

public static class ResourceBuilderExtensions
{

    public static IResourceBuilder<KafkaServerResource> WithHealthCheck(this IResourceBuilder<KafkaServerResource> builder, IEnumerable<string> topics)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new KafkaHealthCheck(topics, cs)));
    }

}
