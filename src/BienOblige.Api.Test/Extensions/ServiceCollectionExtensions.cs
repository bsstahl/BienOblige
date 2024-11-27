using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace BienOblige.Api.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseTestServices<T>(this IServiceCollection services, IConfiguration config, ITestOutputHelper output)
    {
        return services
            .AddLogging(b => b.AddXUnit(output))
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<Mocks.MockHttpMessageHandler>()
            .AddSingleton<Mocks.MockHttpClient>()
            .AddSingleton<HttpClient>(s => s.GetRequiredService<Mocks.MockHttpClient>())
            .AddSingleton<ApiClient.Activities>(s => new ApiClient.Activities(
                s.GetRequiredService<ILogger<T>>(),
                s.GetRequiredService<IConfiguration>(),
                s.GetRequiredService<Mocks.MockHttpClient>()));
    }
}
