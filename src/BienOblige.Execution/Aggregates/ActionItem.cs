using BienOblige.Execution.ValueObjects;
using BienOblige.ValueObjects;

namespace BienOblige.Execution.Aggregates;

public class ActionItem
{
    public NetworkIdentity Id { get; set; }
    public Title Title { get; set; }
    public Content Content { get; set; }

    public NetworkIdentity? ParentId { get; set; }

    public Actor? Actor { get; set; }

    public ActionItem(Title title, Content content)
        : this(NetworkIdentity.New(), title, content)
    { }

    public ActionItem(NetworkIdentity id, Title title, Content content)
    {
        this.Id = id;
        this.Title = title;
        this.Content = content;
    }
}
