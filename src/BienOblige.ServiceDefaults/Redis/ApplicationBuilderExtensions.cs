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
                .WithLifetime(containerLifetime)
                .WithArgs("--notify-keyspace-events KEA")
            .WithRedisCommander()
                .WithLifetime(containerLifetime);
    }
}
