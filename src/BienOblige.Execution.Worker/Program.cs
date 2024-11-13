using BienOblige.Execution.Data.Kafka.Constants;
using BienOblige.Execution.Data.Elastic.Extensions;
using BienOblige.Execution.Data.Kafka.Extensions;

namespace BienOblige.Execution.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults();
        builder.AddDefaultHealthChecks();

        builder.Services
            .UseElasticActionItemRepositories()
            .UseKafkaActivityReadRepository()
            .AddHostedService<ExecutionService>();

        builder.AddElasticsearchClient(Constants.ServiceNames.SearchService);

        builder.AddKafkaConsumer<string, string>(Constants.ServiceNames.KafkaService, s => 
        {
            s.Config.GroupId = Config.ExecutionServiceConsumerGroup;
            s.Config.EnableAutoCommit = false;
        });

        var host = builder.Build();
        host.Run();
    }
}
