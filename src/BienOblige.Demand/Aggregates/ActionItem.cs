using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Aggregates;

public class ActionItem
{
    public NetworkIdentity Id { get; set; }

    public ActionItem(NetworkIdentity id)
    {
        this.Id = id;
    }
}
