using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BienOblige.ServiceDefaults.Serilog;

public static class HostApplicationBuilderExtensions
{
    internal static IHostApplicationBuilder ConfigureSerilog(this IHostApplicationBuilder builder)
    {
        // Removes the built-in logging providers
        builder.Logging.ClearProviders();

        // Including the writeToProviders=true parameter allows the OpenTelemetry logger to still be written to
        builder.Services.AddSerilog((_, loggerConfiguration) =>
        {
            // Configure Serilog as desired here for every project (or use IConfiguration for configuration variations between projects)
            loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        }, writeToProviders: true);

        return builder;
    }

}
