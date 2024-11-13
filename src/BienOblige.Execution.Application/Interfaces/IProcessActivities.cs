using BienOblige.Execution.Application.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IProcessActivities
{
    Task Process(Activity activity, Dictionary<string, string> headers, DateTimeOffset timestamp);
}
