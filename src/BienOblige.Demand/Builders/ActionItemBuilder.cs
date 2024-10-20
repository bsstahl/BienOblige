using BienOblige.ValueObjects;
using BienOblige.Demand.Aggregates;

namespace BienOblige.Demand.Builders;

public class ActionItemBuilder
{
    private NetworkIdentity? _id;

    public ActionItem Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        return new ActionItem(_id)
        { };
    }

    public ActionItemBuilder Id(string id)
    {
        _id = NetworkIdentity.From(id);
        return this;
    }
}
