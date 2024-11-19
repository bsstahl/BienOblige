using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Application.Interfaces;

public interface IPublishActivityCommands
{
    Task<NetworkIdentity> Publish(Activity activity);
}
