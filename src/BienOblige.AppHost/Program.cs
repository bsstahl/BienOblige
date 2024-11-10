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

        // To connect to an existing Kafka server,
        // call AddConnectionString instead of WithReference
        var apiService = builder
            .AddProject<Projects.BienOblige_ApiService>(Constants.ServiceNames.ApiService)
            .WithReference(kafka)
            .WaitFor(kafka);

        var workerService = builder
            .AddProject<Projects.BienOblige_WorkerService>(Constants.ServiceNames.WorkerService)
            .WithReference(kafka)
            .WaitFor(kafka);

        builder.Build().Run();
    }
}