using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;

namespace BienOblige.Api.Builders;

public class ActionItemBuilder
{
    public Uri? _id;
    public string? _name;
    public string? _content;
    public Actor? _generator;
    public NetworkObject? _target;
    public NetworkObject? _parent;
    public IEnumerable<CompletionMethod> _completionMethods = new List<CompletionMethod>();

    public ActionItem Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_name, nameof(_name));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_content, nameof(_content));

        // TODO: Implement remaining properties
        return new ActionItem(_id.ToString(), _name, _content)
        {
            Generator = _generator,
            Target = _target,
            Parent = _parent,
            CompletionMethods = _completionMethods
        };
    }

    public ActionItemBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ActionItemBuilder Id(string value)
    {
        return this.Id(new Uri(value));
    }

    public ActionItemBuilder Id(Uri value)
    {
        _id = value;
        return this;
    }

    public ActionItemBuilder Name(string value)
    {
        _name = value;
        return this;
    }

    public ActionItemBuilder Content(string value)
    {
        _content = value;
        return this;
    }
}
