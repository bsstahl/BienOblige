using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Aggregates;

namespace BienOblige.Execution.Application.Processors;

public class UpdateActivityHandler : IProcessActivities
{
    public Task Process(Activity activity, Dictionary<string, string> headers, DateTimeOffset timestamp)
    {
        throw new NotImplementedException();
    }
}
