namespace BienOblige.Execution.Application.Interfaces;

public interface IManageTransactions<T>
{
    T Content { get; }
    IDictionary<string, string> Headers { get; }
    DateTimeOffset MessageTimestamp { get; }

    Task Commit();
}
