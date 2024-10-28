using BienOblige.Execution.ValueObjects;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Aggregates;

public class ActionItem
{
    public NetworkIdentity Id { get; set; }
    public Title Title { get; set; }

    public Actor? Actor { get; set; }


    public ActionItem(NetworkIdentity id, Title title)
    {
        this.Id = id;
        this.Title = title;
    }
}
