using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.ServiceDefaults.Redis;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<RedisResource> UseBienObligeRedis(
        this IDistributedApplicationBuilder appBuilder,
        string serviceName, ContainerLifetime? lifetime = null)
    {
        var containerLifetime = lifetime ?? ContainerLifetime.Session;
        return appBuilder
            .AddRedis(serviceName)
            .WithContainerName($"BienOblige_{serviceName}")
            .WithLifetime(containerLifetime)
            .WithArgs("--notify-keyspace-events KEA")
            .WithRedisCommander(configureContainer: r => r
                .WithLifetime(containerLifetime)
                .WithContainerName($"BienOblige_ui_{serviceName}"));
    }
}
