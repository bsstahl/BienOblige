using BienOblige.Execution.Data.Kafka.Constants;

namespace BienOblige.Execution.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<ExecutionService>();

        // builder.AddServiceDefaults();
        builder.AddKafkaConsumer<string, string>(Constants.ServiceNames.KafkaService, s => 
        {
            s.Config.GroupId = Config.ExecutionServiceConsumerGroup;
        });

        var host = builder.Build();
        host.Run();
    }
}
