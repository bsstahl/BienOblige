using BienOblige.Api.Entities;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Extensions;
using BienOblige.Api.Interfaces;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class ActionItemBuilder
{
    private List<KeyValuePair<string?, string>>? _context;

    private Uri? _id;
    private string? _name;
    private string? _content;
    private MimeType? _mediaType;
    private Actor? _generator;
    private ObjectBuilder? _targetBuilder;
    private ObjectBuilder? _locationBuilder;
    private DateTimeOffset? _endTime;
    private Uri? _parent;
    private DateTimeOffset? _published;

    private ObjectBuilder? _attributedToBuilder;
    private ObjectBuilder? _audienceBuilder;


    private List<CompletionMethod> _completionMethods = new List<CompletionMethod>([CompletionMethod.Manual]);
    private List<NetworkIdentity>? _prerequisites;

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
        else if (parentActivityType.Equals(ActivityType.Add))
        {
            ArgumentNullException.ThrowIfNull(_locationBuilder, nameof(_locationBuilder));
        }

        var results = (_children?.Build(parentActivityType) ?? Enumerable.Empty<ActionItem>()).ToList();

        // TODO: Implement remaining properties
        var prerequisites = _prerequisites?.Select(p => p.ToString()).ToList();

        var actionItem = new ActionItem()
        {
            Id = _id,
            Context = _context,
            Name = _name,
            Content = _content,
            MediaType = _mediaType?.ToString(),
            Generator = _generator?.AsNetworkObject(),
            Target = _targetBuilder?.Build(),
            AttributedTo = _attributedToBuilder?.Build(),
            Audience = _audienceBuilder?.Build(),
            Parent = _parent?.ToString(),
            CompletionMethods = _completionMethods,
            EndTime = _endTime,
            Published = _published,
            AdditionalProperties = _additionalProperties,
            Prerequisites = prerequisites
        };

        if (_locationBuilder is not null)
        {
            actionItem.Location ??= new();
            actionItem.Location.Add(_locationBuilder.Build());
        }

        results.Add(actionItem);

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

    public ActionItemBuilder Id(NetworkIdentity value)
    {
        _id = value.Value;
        return this;
    }

    public ActionItemBuilder Name(string? value)
    {
        _name = value;
        return this;
    }


    public ActionItemBuilder Content(string? content, string? mediaType)
    {
        return this.Content(content, string.IsNullOrWhiteSpace(mediaType) ? null : MimeType.From(mediaType));
    }

    public ActionItemBuilder Content(string? content, MimeType? mediaType)
    {
        if (!string.IsNullOrWhiteSpace(content) && (mediaType is not null))
        {
            _content = content;
            _mediaType = mediaType;
            return this;
        }
        else if (string.IsNullOrEmpty(content) && (mediaType is null))
            return this; // Do nothing here -- this means the values were never set in the parent and that is ok
        else if (string.IsNullOrEmpty(content))
            throw new ArgumentNullException(nameof(content), "Content must be provided if media type is provided.");
        else
            throw new ArgumentNullException(nameof(mediaType), "Media type must be provided if content is provided.");
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
        _targetBuilder = value;
        return this;
    }

    public ActionItemBuilder Location(NetworkObject value)
    {
        return this.Location(value.AsObjectBuilder());
    }

    public ActionItemBuilder Location(ObjectBuilder value)
    {
        _locationBuilder = value;
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

    public ActionItemBuilder AttributedTo(NetworkObject value)
    {
        return this.AttributedTo(value.AsObjectBuilder());
    }

    public ActionItemBuilder AttributedTo(ObjectBuilder value)
    {
        _attributedToBuilder = value;
        return this;
    }

    public ActionItemBuilder Audience(ActorBuilder value)
    {
        return this.Audience(value.Build().AsObjectBuilder());
    }

    public ActionItemBuilder Audience(NetworkObject value)
    {
        return this.Audience(value.AsObjectBuilder());
    }

    public ActionItemBuilder Audience(ObjectBuilder value)
    {
        _audienceBuilder = value;
        return this;
    }

    public ActionItemBuilder AddPrerequisite(NetworkIdentity actionItemId)
    {
        _prerequisites ??= new List<NetworkIdentity>();
        _prerequisites.Add(actionItemId);
        return this;
    }

    public ActionItemBuilder ClearPrerequisites()
    {
        _prerequisites = null;
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
