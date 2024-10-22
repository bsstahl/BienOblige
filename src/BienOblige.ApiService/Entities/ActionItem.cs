namespace BienOblige.ApiService.Entities;

public class ActionItem
{
    public string Id { get; set; }

    public ActionItem(string id)
    {
        this.Id = id;
    }

    public Execution.Aggregates.ActionItem AsAggregate()
    {
        return new Execution.Aggregates.ActionItem(
            ValueObjects.NetworkIdentity.From(this.Id));
    }

    public static ActionItem From(Execution.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString());
    }

}
