using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Enumerations;

namespace BienOblige.Execution.Application.Interfaces;

public interface ICreateActivities
{
    Task<IEnumerable<NetworkIdentity>> Create(ActivityType activityType, IEnumerable<ActionItem> items, Actor actor, string correlationId);
}
