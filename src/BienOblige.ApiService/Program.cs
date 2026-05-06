// using BienOblige.ApiService.Extensions;
using BienOblige.ApiService.Configuration;
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

// Configure Bearer Token Authentication options
builder.Services.Configure<BearerTokenAuthenticationOptions>(
    builder.Configuration.GetSection(BearerTokenAuthenticationOptions.SectionName));

// Add Generic services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

var app = builder.Build();

app.CreateTopicIfNotExist(BienOblige.Execution.Data.Kafka.Constants.Topics.CommandChannelName);

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// TODO: Restore Custom HTTP request pipeline components.
app.UseBearerTokenAuthentication();
app.UseBienObligeValidation();

// Enable attribute based routing
app.UseRouting();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
