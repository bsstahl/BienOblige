using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Data.Kafka.Extensions;

const string kafkaServiceName = "kafka";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddKafkaProducer<string, string>(kafkaServiceName);

// Add Application services to the container.
builder.Services.UseExecutionClient();
builder.Services.UseKafkaActionItemRepositories();

// Add Generic services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseCorrelation();
app.ConvertActionItemToCollection();

// Enable attribute based routing
app.UseRouting();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
