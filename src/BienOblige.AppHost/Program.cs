using BienOblige.AppHost.Kafka;

var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder
    .AddKafka("kafka")
    .WithKafkaUI()
    .WithHealthCheck(new[]
    {
        "execution_command_private",
        "execution_actionitems_public",
        "execution_compliance_public"
    });

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
