using BienOblige.Api.Entities;

namespace BienOblige.Api.Builders;

public class ActionItemCollectionBuilder
{
    private readonly List<ActionItemBuilder> _builders = new();

    public IEnumerable<ActionItem> Build()
    {
        return _builders.Select(x => x.Build());
    }

    public ActionItemCollectionBuilder Add(ActionItemBuilder builder)
    {
        _builders.Add(builder);
        return this;
    }
}
