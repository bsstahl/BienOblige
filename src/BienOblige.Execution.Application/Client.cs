using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application;

public class Client
{
    private readonly ILogger _logger;
    private readonly IPublishActivityCommands _activityPublisher;

    public Client(ILogger<Client> logger, IPublishActivityCommands activityPublisher)
    {
        _logger = logger;
        _activityPublisher = activityPublisher;
    }

    public async Task<NetworkIdentity> PublishActivityCommand(Activity activity)
    {
        // TODO: Add error handling
        ArgumentNullException.ThrowIfNull(activity);
        ArgumentNullException.ThrowIfNull(activity.Target);
        ArgumentNullException.ThrowIfNull(activity.Actor);

        return await _activityPublisher.Publish(activity);
    }

}
