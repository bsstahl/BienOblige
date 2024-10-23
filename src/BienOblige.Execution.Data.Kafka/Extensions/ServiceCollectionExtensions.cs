using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Execution.Data.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseKafkaActionItemRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICreateActionItems, ActionItemRepository>();
    }
}
