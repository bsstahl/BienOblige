using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Enumerations;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Application.Aggregates;

public class Activity(NetworkIdentity id, ActivityType activityType, Actor activityActor, ActionItem item, DateTimeOffset published)
{
    public NetworkIdentity Id { get; set; } = id;
    public ActivityType ActivityType { get; private set; } = activityType;
    public Actor Actor { get; set; } = activityActor;
    public ActionItem ActionItem { get; set; } = item;
    public DateTimeOffset Published { get; set; } = published;
}
