using BienOblige.ServiceDefaults.Kafka;

namespace BienOblige.AppHost;

internal class Program
{
    const string kafkaServiceName = "kafka";

    private static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var kafka = builder.UseBienObligeKafka(kafkaServiceName);

        // To connect to an existing Kafka server,
        // call AddConnectionString instead of WithReference
        var apiService = builder
            .AddProject<Projects.BienOblige_ApiService>("api")
            .WithReference(kafka)
            .WaitFor(kafka);

        //builder.AddProject<Projects.BienOblige_Web>("webfrontend")
        //    .WithExternalHttpEndpoints()
        //    .WithReference(apiService);

        builder.Build().Run();
    }
}