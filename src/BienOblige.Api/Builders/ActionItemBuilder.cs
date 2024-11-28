using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;
using BienOblige.Api.Interfaces;

namespace BienOblige.Api.Builders;

public class ActionItemBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _id;
    private string? _name;
    private string? _content;
    private Actor? _generator;
    private ObjectBuilder? _target;
    private ObjectBuilder? _location;
    private DateTimeOffset? _endTime;
    private Uri? _parent;
    private DateTimeOffset? _published;
    private List<CompletionMethod> _completionMethods = new List<CompletionMethod>([CompletionMethod.Manual]);

    private Dictionary<string, object> _additionalProperties = new();

    private ActionItemCollectionBuilder? _children;
    
    private readonly Uri _instanceBaseUri;

    public IEnumerable<KeyValuePair<string?, string>>? GetContext() => _context;
    public Uri? GetId() => _id;


    public ActionItemBuilder()
        : this(new Uri(Constants.Path.DefaultBaseUri))
    { }

    public ActionItemBuilder(Uri instanceBaseUri)
    {
        _instanceBaseUri = instanceBaseUri;
    }

    public IEnumerable<ActionItemBuilder> GetAllBuilders(ActionItemBuilder? parentBuilder)
    {
        var result = (_children?.GetAllBuilders(this) ?? Array.Empty<ActionItemBuilder>()).ToList();
        
        // Update this builder with the information from the parent
        _parent ??= parentBuilder?.GetId();
        _children = null;  // We've flattened the hierarchy

        result.Add(this);
        return result;
    }

    public IEnumerable<ActionItem> Build(ActivityType parentActivityType)
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));

        if (parentActivityType.Equals(ActivityType.Create))
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(_name, nameof(_name));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(_content, nameof(_content));
        }

        var results = (_children?.Build(parentActivityType) ?? Enumerable.Empty<ActionItem>()).ToList();

        // TODO: Implement remaining properties
        results.Add(new ActionItem()
        {
            Id = _id.ToString(),
            Context = _context,
            Name = _name,
            Content = _content,
            Generator = _generator,
            Target = _target?.Build(),
            Parent = _parent?.ToString(),
            CompletionMethods = _completionMethods,
            EndTime = _endTime,
            Location = _location?.Build(),
            Published = _published,
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

    public ActionItemBuilder Location(NetworkObject value)
    {
        return this.Location(value.AsObjectBuilder());
    }

    public ActionItemBuilder Location(ObjectBuilder value)
    {
        _location = value;
        return this;
    }

    public ActionItemBuilder Published(DateTimeOffset? value, bool overwrite = true)
    {
        if (overwrite)
            _published = value;
        else
            _published ??= value;
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
