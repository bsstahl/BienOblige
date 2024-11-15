using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Data.Kafka.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddKafkaProducer<string, string>(BienOblige.Constants.ServiceNames.KafkaService);
builder.AddElasticsearchClient(BienOblige.Constants.ServiceNames.SearchService);

// Add Application services to the container.
builder.Services.UseExecutionClient();
builder.Services.UseKafkaActivityWriteRepository();

// Add Generic services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var app = builder.Build();

app.CreateTopicIfNotExist(BienOblige.Execution.Data.Kafka.Constants.Topics.CommandChannelName);

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Custom HTTP request pipeline components.
app.UseCorrelation();
app.ValidateMetadata();
app.ValidateActionItem();

// Enable attribute based routing
app.UseRouting();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
