using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActionItemCollectionBuilder : List<ActionItemBuilder>
{
    public IEnumerable<ActionItem> Build(ActivityType parentActivityType)
    {
        return this.SelectMany(x => x.Build(parentActivityType));
    }

    public IEnumerable<ActionItemBuilder> GetAllBuilders(ActionItemBuilder? parentBuilder = null)
    {
        return this.SelectMany(x => x.GetAllBuilders(parentBuilder));
    }

    public new ActionItemCollectionBuilder Add(ActionItemBuilder builder)
    {
        base.Add(builder);
        return this;
    }

    public ActionItemCollectionBuilder AssignIds(Uri instanceBaseUri)
    {
        this.ToList().ForEach(b => b.AssignId(instanceBaseUri));
        return this;
    }

    public ActionItemCollectionBuilder Parent(Uri parentId)
    {
        this.ToList().ForEach(b => b.Parent(parentId));
        return this;
    }

    public ActionItemCollectionBuilder Published(DateTimeOffset? published, bool overwrite = true)
    {
        this.ToList().ForEach(b => b.Published(published, overwrite));
        return this;
    }
}
