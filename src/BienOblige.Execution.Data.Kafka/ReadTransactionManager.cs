using Confluent.Kafka;

namespace BienOblige.Execution.Application;

public class ReadTransactionManager<T> : Interfaces.IManageTransactions<T>
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ConsumeResult<string, string> _consumeResult;

    public T Content { get; set; }
    public IDictionary<string, string> Headers { get; set; }
    public DateTimeOffset MessageTimestamp { get; set; }


    public ReadTransactionManager(IConsumer<string, string> consumer, 
        ConsumeResult<string, string> consumeResult, 
        T content, IDictionary<string, string> headers, DateTimeOffset timestamp)
    {
        _consumer = consumer;
        _consumeResult = consumeResult;
        this.Content = content;
        this.Headers = headers;
        this.MessageTimestamp = timestamp;
    }

    public Task Commit()
    {
        _consumer.Commit(_consumeResult);
        return Task.CompletedTask;
    }
}
