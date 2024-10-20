using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Application.Test.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Demand.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection UseMockActionItemRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGetActionItems, MockActionItemReader>()
            .AddSingleton<ICreateActionItems, MockActionItemCreator>();
    }
}
