using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.Builders;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Builders;

public class ActionItemBuilder
{
    private NetworkIdentity? _id;
    private Name? _title;
    private Content? _content;

    private ActorBuilder? _actorBuilder;

    public ActionItem Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_title, nameof(_title));
        ArgumentNullException.ThrowIfNull(_content, nameof(_content));

        var result = new ActionItem(_id, _title, _content);
        if (_actorBuilder is not null)
            result.LastUpdatedBy = _actorBuilder.Build();
        return result;
    }

    public ActionItemBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ActionItemBuilder Id(string value)
    {
        return this.Id(NetworkIdentity.From(value));
    }

    public ActionItemBuilder Id(NetworkIdentity value)
    {
        _id = value;
        return this;
    }

    public ActionItemBuilder Name(string value)
    {
        return this.Title(ActivityStream.ValueObjects.Name.From(value));
    }

    public ActionItemBuilder Title(Name value)
    {
        _title = value;
        return this;
    }

    public ActionItemBuilder Content(string value)
    {
        return this.Content(ActivityStream.ValueObjects.Content.From(value));
    }

    public ActionItemBuilder Content(Content value)
    {
        _content = value;
        return this;
    }

    public ActionItemBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

}
