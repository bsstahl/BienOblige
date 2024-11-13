using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Execution.Data.Elastic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseElasticActionItemRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGetActionItems, ActionItemReadRepository>()
            .AddSingleton<IUpdateActionItems, ActionItemWriteRepository>();
    }

}
