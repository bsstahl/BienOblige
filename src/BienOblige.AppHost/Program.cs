using BienOblige.ServiceDefaults.Kafka;
using BienOblige.ServiceDefaults.Elastic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BienOblige.ServiceDefaults.Redis;

namespace BienOblige.AppHost;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = DistributedApplication
            .CreateBuilder(args);

        // Generic service registration
        var search = builder.UseBienObligeElasticSearch(Constants.ServiceNames.SearchService);
        var kafka = builder.UseBienObligeKafka(Constants.ServiceNames.KafkaService);
        var cache = builder.UseBienObligeRedis(Constants.ServiceNames.CacheService);

        // To connect to an existing server, call AddConnectionString instead of WithReference below

        // Custom service registration
        var apiService = builder
            .AddProject<Projects.BienOblige_ApiService>(Constants.ServiceNames.ApiService)
            .WithReference(kafka)
            .WaitFor(kafka);

        var executionService = builder
            .AddProject<Projects.BienOblige_Execution_Worker>(Constants.ServiceNames.ExecutionService)
            .WithReference(kafka)
            .WithReference(cache)
            .WaitFor(kafka)
            .WaitFor(cache);

        var cacheConnector = builder
            .AddProject<Projects.BienOblige_Execution_CacheConnector>(Constants.ServiceNames.CacheConnectorService)
            .WithReference(cache)
            .WithReference(kafka)
            .WaitFor(cache)
            .WaitFor(kafka);

        builder.Build().Run();
    }
}