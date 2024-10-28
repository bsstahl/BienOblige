namespace BienOblige.ApiService.Entities;

public class ActionItem(string id, string title)
{
    public string Id { get; set; } = id;
    public string Title { get; set; } = title;

    public Execution.Aggregates.ActionItem AsAggregate()
    {
        return new Execution.Aggregates.ActionItem(
            ValueObjects.NetworkIdentity.From(this.Id),
            Execution.ValueObjects.Title.From(this.Title))
        { 
        };
    }

    public static ActionItem From(Execution.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString(), item.Title.Value);
    }

}
