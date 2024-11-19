using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Application.Interfaces;

public interface IProcessActivities
{
    Task Process(Activity activity, Dictionary<string, string> headers, DateTimeOffset timestamp);
}
