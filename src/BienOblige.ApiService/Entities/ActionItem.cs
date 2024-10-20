namespace BienOblige.ApiService.Entities;

public class ActionItem
{
    public string Id { get; set; }

    public ActionItem(string id)
    {
        this.Id = id;
    }

    public Demand.Aggregates.ActionItem AsAggregate()
    {
        return new Demand.Aggregates.ActionItem(
            Demand.ValueObjects.NetworkIdentity.From(this.Id));
    }

    public static ActionItem From(Demand.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString());
    }

}
