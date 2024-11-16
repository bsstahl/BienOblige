using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.ValueObjects;

namespace BienOblige.Execution.Aggregates;

public class ActionItem
{
    public NetworkIdentity Id { get; set; }
    public Title Title { get; set; }
    public Content Content { get; set; }

    public NetworkObject? Parent { get; set; }
    public NetworkObject? Target { get; set; }

    public Actor? Generator { get; set; }
    public Actor? LastUpdatedBy { get; set; }

    public DateTimeOffset LastUpdatedAt { get; set; }

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
