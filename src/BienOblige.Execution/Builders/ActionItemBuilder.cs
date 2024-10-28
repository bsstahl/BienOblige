using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.ValueObjects;

namespace BienOblige.Execution.Builders;

public class ActionItemBuilder
{
    private NetworkIdentity? _id;
    private Title? _title;

    private ActorBuilder? _actorBuilder;

    public ActionItem Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_title, nameof(_title));

        var result = new ActionItem(_id, _title);
        if (_actorBuilder is not null)
            result.Actor = _actorBuilder.Build();
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

    public ActionItemBuilder Title(string value)
    {
        return this.Title(ValueObjects.Title.From(value));
    }

    public ActionItemBuilder Title(Title value)
    {
        _title = value;
        return this;
    }

    public ActionItemBuilder Actor(ActorBuilder value)
    {
        _actorBuilder = value;
        return this;
    }

}
