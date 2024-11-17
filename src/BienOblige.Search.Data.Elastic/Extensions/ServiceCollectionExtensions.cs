using BienOblige.Search.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Search.Data.Elastic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseElasticActionItemRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFindActionItems, ActionItemReadRepository>()
            .AddSingleton<IUpdateActionItems, ActionItemWriteRepository>();
    }

}
