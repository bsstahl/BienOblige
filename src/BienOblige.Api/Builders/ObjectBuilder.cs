using BienOblige.Api.Entities;
using BienOblige.Api.Extensions;
using BienOblige.Api.ValueObjects;

namespace BienOblige.Api.Builders;

public class ObjectBuilder
{
    private Uri? _id;
    private List<string> _objectTypes = new();
    private List<KeyValuePair<string?, string>>? _context;
    private List<ObjectBuilder> _attachmentBuilders = new();
    private ObjectBuilder? _attributedToBuilder;
    private ObjectBuilder? _audienceBuilder;
    private readonly List<ObjectBuilder> _bccBuilders = new();
    private ObjectBuilder? _btoBuilder;
    private ObjectBuilder? _ccBuilder;
    private string? _content;
    private TimeSpan? _duration;
    private DateTimeOffset? _endTime;
    private ObjectBuilder? _generatorBuilder;
    private ObjectBuilder? _iconBuilder;
    private ObjectBuilder? _imageBuilder;
    private readonly List<ObjectBuilder> _inReplyToBuilders = new();
    private readonly List<ObjectBuilder> _locationBuilders = new();
    private MimeType? _mediaType;
    private string? _name;
    private ObjectBuilder? _previewBuilder;
    private DateTimeOffset? _published;
    private readonly List<ObjectBuilder> _repliesBuilders = new();
    private DateTimeOffset? _startTime;
    private string? _summary;
    private readonly List<ObjectBuilder> _tagBuilders = new();
    private ObjectBuilder? _toBuilder;
    private DateTimeOffset? _updated;
    private readonly List<Uri> _urls = new();
    private readonly Dictionary<string, object> _additionalProperties = new();

    public NetworkObject Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));

        return !_objectTypes.Any()
            ? throw new InvalidOperationException("An object type must be specified.")
            : new NetworkObject
            {
                ObjectId = _id,
                ObjectType = _objectTypes,
                Attachments = _attachmentBuilders.Build(),
                AttributedTo = _attributedToBuilder?.Build(),
                Audience = _audienceBuilder?.Build(),
                Bcc = _bccBuilders.Build(),
                Bto = _btoBuilder?.Build(),
                Cc = _ccBuilder?.Build(),
                Content = _content,
                Context = _context,
                Duration = _duration,
                EndTime = _endTime,
                Generator = _generatorBuilder?.Build(),
                Icon = _iconBuilder?.Build(),
                Image = _imageBuilder?.Build(),
                InReplyTo = _inReplyToBuilders.Build(),
                Location = _locationBuilders.Build(),
                MediaType = _mediaType?.ToString(),
                Name = _name,
                Preview = _previewBuilder?.Build(),
                Published = _published,
                Replies = _repliesBuilders.Build(),
                StartTime = _startTime,
                Summary = _summary,
                Tags = _tagBuilders.Build(),
                To = _toBuilder?.Build(),
                LastUpdatedAt = _updated,
                Url = _urls,
                AdditionalProperties = _additionalProperties
            };
    }

    public ObjectBuilder AddContext(string? key, string value)
    {
        return this.AddContext([new KeyValuePair<string?, string>(key, value)]);
    }

    public ObjectBuilder AddContext(IEnumerable<KeyValuePair<string?, string>>? values)
    {
        if (values?.Any() ?? false)
        {
            _context ??= new();
            _context.AddRange(values);
        }
        return this;
    }

    public ObjectBuilder ClearContext()
    {
        _context = null;
        return this;
    }

    public ObjectBuilder Id(Guid id, string entityTypeName)
    {
        return this.Id($"{Constants.Path.DefaultBaseUri}/{entityTypeName}/{id.ToString()}");
    }

    public ObjectBuilder Id(string id)
    {
        return this.Id(new Uri(id));
    }

    public ObjectBuilder Id(NetworkIdentity id)
    {
        return this.Id(id.Value);
    }

    public ObjectBuilder Id(Uri id)
    {
        _id = id;
        return this;
    }

    public ObjectBuilder AddObjectType(string value)
    {
        _objectTypes.Add(value);
        return this;
    }

    public ObjectBuilder AddObjectTypes(IEnumerable<string>? values)
    {
        _objectTypes.AddRange(values ?? Array.Empty<string>());
        return this;
    }

    public ObjectBuilder ClearObjectTypes()
    {
        _objectTypes.Clear();
        return this;
    }

    public ObjectBuilder AddAttachment(ObjectBuilder builder)
    {
        if (builder is not null)
            _attachmentBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddAttachments(IEnumerable<ObjectBuilder> builders)
    {
        _attachmentBuilders.AddRange(builders);
        return this;
    }

    public ObjectBuilder ClearAttachments()
    {
        _attachmentBuilders.Clear();
        return this;
    }

    public ObjectBuilder AttributedTo(ObjectBuilder? builder)
    {
        _attributedToBuilder = builder;
        return this;
    }

    public ObjectBuilder Audience(ObjectBuilder? builder)
    {
        _audienceBuilder = builder;
        return this;
    }

    public ObjectBuilder AddBcc(ObjectBuilder? builder)
    {
        if (builder is not null)
            _bccBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddBccs(IEnumerable<ObjectBuilder> builders)
    {
        _bccBuilders.AddRange(builders);
        return this;
    }

    public ObjectBuilder ClearBcc()
    {
        _bccBuilders.Clear();
        return this;
    }

    public ObjectBuilder Bto(ObjectBuilder? builder)
    {
        _btoBuilder = builder;
        return this;
    }

    public ObjectBuilder Cc(ObjectBuilder? builder)
    {
        _ccBuilder = builder;
        return this;
    }

    public ObjectBuilder Content(string? content, string? mediaType)
    {
        return this.Content(content, string.IsNullOrWhiteSpace(mediaType) ? null : MimeType.From(mediaType));
    }

    public ObjectBuilder Content(string? content, MimeType? mediaType)
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

    public ObjectBuilder Duration(TimeSpan? value)
    {
        _duration = value;
        return this;
    }

    public ObjectBuilder EndTime(DateTimeOffset? value)
    {
        _endTime = value;
        return this;
    }

    public ObjectBuilder Generator(ObjectBuilder? builder)
    {
        _generatorBuilder = builder;
        return this;
    }

    public ObjectBuilder Icon(ObjectBuilder? builder)
    {
        _iconBuilder = builder;
        return this;
    }

    public ObjectBuilder Image(ObjectBuilder? builder)
    {
        _imageBuilder = builder;
        return this;
    }

    public ObjectBuilder AddInReplyTo(ObjectBuilder? builder)
    {
        if (builder is not null)
            _inReplyToBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddInReplyTo(IEnumerable<ObjectBuilder>? builders)
    {
        _inReplyToBuilders.AddRange(builders ?? []);
        return this;
    }

    public ObjectBuilder ClearInReplyTo()
    {
        _inReplyToBuilders.Clear();
        return this;
    }

    public ObjectBuilder AddLocation(ObjectBuilder? builder)
    {
        if (builder is not null)
            _locationBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddLocations(IEnumerable<ObjectBuilder>? builders)
    {
        _locationBuilders.AddRange(builders ?? []);
        return this;
    }


    public ObjectBuilder ClearLocation()
    {
        _locationBuilders.Clear();
        return this;
    }

    public ObjectBuilder Name(string? value)
    {
        _name = value;
        return this;
    }

    public ObjectBuilder Preview(ObjectBuilder? builder)
    {
        _previewBuilder = builder;
        return this;
    }

    public ObjectBuilder Published(DateTimeOffset? value)
    {
        _published = value;
        return this;
    }

    public ObjectBuilder AddReply(ObjectBuilder? builder)
    {
        if (builder is not null)
            _repliesBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddReplies(IEnumerable<ObjectBuilder> builders)
    {
        _repliesBuilders.AddRange(builders);
        return this;
    }

    public ObjectBuilder ClearReplies()
    {
        _repliesBuilders.Clear();
        return this;
    }

    public ObjectBuilder StartTime(DateTimeOffset? value)
    {
        _startTime = value;
        return this;
    }

    public ObjectBuilder Summary(string? value)
    {
        _summary = value;
        return this;
    }

    public ObjectBuilder AddTag(ObjectBuilder? builder)
    {
        if (builder is not null)
            _tagBuilders.Add(builder);
        return this;
    }

    public ObjectBuilder AddTags(IEnumerable<ObjectBuilder> builders)
    {
        _tagBuilders.AddRange(builders);
        return this;
    }

    public ObjectBuilder ClearTags()
    {
        _tagBuilders.Clear();
        return this;
    }

    public ObjectBuilder To(ObjectBuilder? builder)
    {
        _toBuilder = builder;
        return this;
    }

    public ObjectBuilder LastUpdatedAt(DateTimeOffset? value)
    {
        _updated = value;
        return this;
    }

    public ObjectBuilder AddUrl(string value)
    {
        return this.AddUrl(new Uri(value));
    }

    public ObjectBuilder AddUrl(Uri value)
    {
        _urls.Add(value);
        return this;
    }

    public ObjectBuilder AddUrls(IEnumerable<Uri>? values)
    {
        _urls.AddRange(values ?? []);
        return this;
    }

    public ObjectBuilder ClearUrls()
    {
        _urls.Clear();
        return this;
    }

    public ObjectBuilder AddAdditionalProperties(IDictionary<string, object> values)
    {
        values.ToList().ForEach(v => this.AddAdditionalProperty(v.Key, v.Value));
        return this;
    }

    public ObjectBuilder AddAdditionalProperty(string key, object? value)
    {
        if (value is not null)
            _additionalProperties.Add(key, value);
        return this;
    }

}