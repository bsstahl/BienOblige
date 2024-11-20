using BienOblige.ActivityStream.ValueObjects;
using BienOblige.ActivityStream.Enumerations;

namespace BienOblige.ActivityStream.Aggregates;

public class Activity : NetworkObject
{
    const string _defaultTypeName = "[ bienoblige:ActionItem, Object ]";

    public ActivityType ActivityType { get; set; }
    public Actor Actor { get; set; }
    public ActionItem ActionItem { get; set; }

    public Activity(NetworkIdentity id, ActivityType activityType, Actor activityActor, ActionItem item, DateTimeOffset published)
        : base(id, TypeName.From(_defaultTypeName))
    {
        this.ActivityType = activityType;
        this.Actor = activityActor;
        this.ActionItem = item;
        base.Published = published;
    }
}
