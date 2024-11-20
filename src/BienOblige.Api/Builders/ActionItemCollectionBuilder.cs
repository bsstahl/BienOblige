using BienOblige.Api.Entities;

namespace BienOblige.Api.Builders;

public class ActionItemCollectionBuilder
{
    private readonly List<ActionItemBuilder> _builders = new();

    public IEnumerable<ActionItem> Build(Uri? parentId = null)
    {
        if (parentId is not null)
            _builders.ToList().ForEach(x => x.Parent(parentId));
        return _builders.SelectMany(x => x.Build());
    }

    public ActionItemCollectionBuilder Add(ActionItemBuilder builder)
    {
        _builders.Add(builder);
        return this;
    }

    public ActionItemCollectionBuilder AssignIds(Uri instanceBaseUri)
    {
        _builders.ToList().ForEach(b => b.AssignId(instanceBaseUri));
        return this;
    }
}
