using BienOblige.Execution.Data.Kafka.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.ServiceDefaults.Kafka;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<KafkaServerResource> UseBienObligeKafka(
        this IDistributedApplicationBuilder builder,
        string serviceName)
    {
        // builder.Services.AddHealthChecks().AddCheck<KafkaHealthCheck>(serviceName);

        return builder
            .AddKafka(serviceName)
            .WithKafkaUI();
    }
}
