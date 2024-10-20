var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder
    .AddProject<Projects.BienOblige_ApiService>("api");

//builder.AddProject<Projects.BienOblige_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.Build().Run();
