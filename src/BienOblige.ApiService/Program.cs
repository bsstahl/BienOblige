using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Data.Kafka.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "BienOblige.ApiService")
    .WriteTo.Console()
    .CreateLogger(); 

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddKafkaProducer<string, string>("kafka");

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

// Enable attribute based routing
app.UseRouting();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
