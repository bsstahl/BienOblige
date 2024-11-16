using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.ServiceDefaults.Redis;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<RedisResource> UseBienObligeRedis(
        this IDistributedApplicationBuilder appBuilder,
        string serviceName)
    {
        return appBuilder
            .AddRedis(serviceName)
            .WithArgs("--notify-keyspace-events KEA")
            .WithRedisCommander();
    }
}
