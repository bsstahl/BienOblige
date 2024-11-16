using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Kafka.Constants;
using BienOblige.Execution.Data.Kafka.Extensions;
using BienOblige.Execution.Data.Redis;

namespace BienOblige.Execution.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder
            .AddServiceDefaults()
            .AddKafkaConsumer<string, string>(Constants.ServiceNames.KafkaService, s =>
        {
            // start from the beginning every time
            s.Config.GroupId = $"{Config.ExecutionServiceConsumerGroup}_{Guid.NewGuid()}";
            s.Config.EnableAutoCommit = false;
        });

        builder.Services
            .UseKafkaActivityReadRepository()
            .AddSingleton<IGetActionItems, ReadRepository>()
            .AddSingleton<IUpdateActionItems, WriteRepository>()
            .AddHostedService<ExecutionService>();

        builder.AddRedisClient(Constants.ServiceNames.CacheService);

        var host = builder.Build();
        host.Run();
    }
}
