using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Redis;

namespace BienOblige.Execution.CacheConnector;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults();
        builder.AddRedisClient(Constants.ServiceNames.CacheService);
        builder.AddKafkaProducer<string, string>(Constants.ServiceNames.KafkaService);

        builder.Services
            .AddSingleton<IGetActionItems, ReadRepository>()
            .AddHostedService<ActionItemConnector>();
        var host = builder.Build();

        host.CreateTopicIfNotExist(Data.Kafka.Constants.Topics.ActionItemsPublicChannelName);

        host.Run();
    }
}