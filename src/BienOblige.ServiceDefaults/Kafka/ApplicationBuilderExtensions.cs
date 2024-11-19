using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(
        this IDistributedApplicationBuilder appBuilder,
        string serviceName, ContainerLifetime? lifetime = null)
    {
        var containerLifetime = lifetime ?? ContainerLifetime.Session;
        return appBuilder
            .AddKafka(serviceName).WithLifetime(containerLifetime)
            .WithKafkaUI().WithLifetime(containerLifetime);
    }

}
