using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;

namespace BienOblige.Api.Builders;

public class ActionItemBuilder
{
    private Uri? _id;
    private string? _name;
    private string? _content;
    private Actor? _generator;
    private NetworkObject? _target;
    private Uri? _parent;
    private IEnumerable<CompletionMethod> _completionMethods = new List<CompletionMethod>();

    private ActionItemCollectionBuilder? _children;

    public IEnumerable<ActionItem> Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_name, nameof(_name));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_content, nameof(_content));

        // TODO: Implement remaining properties
        var results = (_children?.Build(_id) ?? Enumerable.Empty<ActionItem>()).ToList();
        results.Add(new ActionItem(_id.ToString(), _name, _content)
        {
            Generator = _generator,
            Target = _target,
            Parent = _parent?.ToString(),
            CompletionMethods = _completionMethods
        });

        return results;
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

    public ActionItemBuilder Parent(Uri id)
    {
        _parent = id;
        return this;
    }

    public ActionItemBuilder Children(ActionItemCollectionBuilder value)
    {
        _children = value;
        return this;
    }

    internal void AssignId(Uri instanceBaseUri)
    {
        _children?.AssignIds(instanceBaseUri);
        _id ??= instanceBaseUri.AsInstanceId();
    }
}
