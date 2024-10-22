using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Execution.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection UseMockRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGetActionItems, MockActionItemReader>()
            .AddSingleton<ICreateActionItems, MockActionItemCreator>();
    }
}
