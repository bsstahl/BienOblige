using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(
        this IDistributedApplicationBuilder appBuilder,
        string serviceName)
    {
        return appBuilder
            .AddKafka(serviceName)
            .WithKafkaUI();
    }
}
