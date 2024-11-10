using BienOblige.ServiceDefaults.Kafka;
using BienOblige.ServiceDefaults.Elastic;

namespace BienOblige.AppHost;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        // Generic service registration
        var kafka = builder.UseBienObligeKafka(Constants.ServiceNames.KafkaService);
        var search = builder.UseBienObligeElasticSearch(Constants.ServiceNames.SearchService);

        // To connect to an existing server, call AddConnectionString instead of WithReference below

        // Custom service registration
        var apiService = builder
            .AddProject<Projects.BienOblige_ApiService>(Constants.ServiceNames.ApiService)
            .WithReference(kafka)
            .WaitFor(kafka);

        var executionService = builder
            .AddProject<Projects.BienOblige_Execution_Worker>(Constants.ServiceNames.ExecutionService)
            .WithReference(kafka)
            .WithReference(search)
            .WaitFor(kafka)
            .WaitFor(search);

        builder.Build().Run();
    }
}