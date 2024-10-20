using BienOblige.Demand.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Demand.Data.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseKafkaActionItemRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGetActionItems, ActionItemRepository>()
            .AddSingleton<ICreateActionItems, ActionItemRepository>();
    }
}
