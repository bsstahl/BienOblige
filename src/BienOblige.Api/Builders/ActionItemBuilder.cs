using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;
using BienOblige.Api.Interfaces;

namespace BienOblige.Api.Builders;

public class ActionItemBuilder
{
    private Uri? _id;
    private string? _name;
    private string? _content;
    private Actor? _generator;
    private ObjectBuilder? _target;
    private DateTimeOffset? _endTime;
    private Uri? _parent;
    private List<CompletionMethod> _completionMethods = new List<CompletionMethod>([CompletionMethod.Manual]);
    private Dictionary<string, object> _additionalProperties = new();

    private ActionItemCollectionBuilder? _children;
    
    private readonly Uri _instanceBaseUri;

    public ActionItemBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public ActionItemBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public IEnumerable<ActionItem> Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_name, nameof(_name));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_content, nameof(_content));

        // TODO: Implement remaining properties
        var results = (_children?.Build(_id) ?? Enumerable.Empty<ActionItem>()).ToList();
        results.Add(new ActionItem()
        {
            Id = _id.ToString(),
            Name = _name,
            Content = _content,
            Generator = _generator,
            Target = _target?.Build(),
            Parent = _parent?.ToString(),
            CompletionMethods = _completionMethods,
            EndTime = _endTime,
            AdditionalProperties = _additionalProperties
        });

        return results;
    }

    public ActionItemBuilder Id(Guid value)
    {
        return this.Id($"{_instanceBaseUri}ActionItem/{value.ToString()}");
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

    public ActionItemBuilder EndTime(DateTimeOffset value)
    {
        _endTime = value;
        return this;
    }

    public ActionItemBuilder Parent(Uri id)
    {
        _parent = id;
        return this;
    }

    public ActionItemBuilder ClearCompletionMethods()
    {
        _completionMethods.Clear();
        return this;
    }

    public ActionItemBuilder AddCompletionMethod(CompletionMethod value)
    {
        _completionMethods.Add(value);
        return this;
    }

    public ActionItemBuilder Children(ActionItemCollectionBuilder value)
    {
        _children = value;
        return this;
    }

    public ActionItemBuilder Target(IActionItemTarget value)
    {
        return this.Target(value.AsNetworkObject());
    }

    public ActionItemBuilder Target(NetworkObject value)
    {
        return this.Target(value.AsObjectBuilder());
    }

    public ActionItemBuilder Target(ObjectBuilder value)
    {
        _target = value;
        return this;
    }

    public ActionItemBuilder ClearAdditionalProperty()
    {
        _additionalProperties.Clear();
        return this;
    }

    public ActionItemBuilder AddAdditionalProperty(string key, object value)
    {
        _additionalProperties.Add(key, value);
        return this;
    }


    internal void AssignId(Uri instanceBaseUri)
    {
        _children?.AssignIds(instanceBaseUri);
        _id ??= instanceBaseUri.AsInstanceId();
    }
}
