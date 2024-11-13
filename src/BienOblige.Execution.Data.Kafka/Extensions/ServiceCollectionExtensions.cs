using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Execution.Data.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseKafkaActivityReadRepository(this IServiceCollection services)
    {
        return services.AddSingleton<IGetActivities, ActivityReadRepository>();
    }

    public static IServiceCollection UseKafkaActivityWriteRepository(this IServiceCollection services)
    {
        return services.AddSingleton<ICreateActivities, ActivityWriteRepository>();
    }
}
