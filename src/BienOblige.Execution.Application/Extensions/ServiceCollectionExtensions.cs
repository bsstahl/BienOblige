using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Execution.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseExecutionClient(this IServiceCollection services)
    {
        return services
            .AddScoped<Client>();
    }
}
