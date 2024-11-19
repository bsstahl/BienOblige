using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application.Interfaces;

namespace BienOblige.Execution.Application.Processors;

public class CreateActivityHandler : IProcessActivities
{
    public Task Process(Activity activity, Dictionary<string, string> headers, DateTimeOffset timestamp)
    {
        throw new NotImplementedException();
    }
}
