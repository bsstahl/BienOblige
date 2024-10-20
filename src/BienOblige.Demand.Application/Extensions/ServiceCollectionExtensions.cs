using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Demand.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseDemandClient(this IServiceCollection services)
    {
        return services.AddScoped<Client>();
    }
}
