using BienOblige.ValueObjects;

namespace BienOblige.Execution.Aggregates;

public class ActionItem
{
    public NetworkIdentity Id { get; set; }

    public ActionItem(NetworkIdentity id)
    {
        this.Id = id;
    }
}
