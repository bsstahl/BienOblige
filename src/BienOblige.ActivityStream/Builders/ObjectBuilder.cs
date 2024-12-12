using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.Extensions;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.ActivityStream.Builders;

public class ObjectBuilder
{
    private NetworkIdentity? _id;
    private List<TypeName>? _typeNames;

    private Content? _content;
    private Name? _name;

    private List<ObjectBuilder>? _attachment;

    // TODO: Complete this implementation
    //private NetworkObject? attributedTo { get; set; }
    //private NetworkObject? audience { get; set; }
    //private NetworkObject? context { get; set; }
    //private NetworkObject? endTime { get; set; }
    //private NetworkObject? generator { get; set; }
    //private NetworkObject? icon { get; set; }
    //private NetworkObject? image { get; set; }
    //private NetworkObject? inReplyTo { get; set; }
    //private NetworkObject? location { get; set; }
    //private NetworkObject? preview { get; set; }
    //private NetworkObject? published { get; set; }
    //private NetworkObject? replies { get; set; }
    //private NetworkObject? startTime { get; set; }
    //private NetworkObject? summary { get; set; }
    //private NetworkObject? tag { get; set; }
    //private NetworkObject? updated { get; set; }
    //private NetworkObject? url { get; set; }
    //private NetworkObject? to { get; set; }
    //private NetworkObject? bto { get; set; }
    //private NetworkObject? cc { get; set; }
    //private NetworkObject? bcc { get; set; }
    //private NetworkObject? mediaType { get; set; }
    //private NetworkObject? duration { get; set; }

    public NetworkObject Build()
    {
        ArgumentNullException.ThrowIfNull(_id, nameof(_id));
        ArgumentNullException.ThrowIfNull(_name, nameof(_name));
        ArgumentNullException.ThrowIfNull(_content, nameof(_content));
        ArgumentNullException.ThrowIfNull(_typeNames, nameof(_typeNames));

        return new NetworkObject()
        {
            Id = _id,
            Name = _name,
            Content = _content,
            ObjectTypeName = _typeNames,
            Attachment = _attachment.BuildCollection()
        };
    }

    public ObjectBuilder Id(Guid value)
    {
        return this.Id($"urn:uid:{value.ToString()}");
    }

    public ObjectBuilder Id(string value)
    {
        return this.Id(NetworkIdentity.From(value));
    }

    public ObjectBuilder Id(NetworkIdentity value)
    {
        _id = value;
        return this;
    }

    public ObjectBuilder Name(string value)
    {
        return this.Title(ActivityStream.ValueObjects.Name.From(value));
    }

    public ObjectBuilder Title(Name value)
    {
        _name = value;
        return this;
    }

    public ObjectBuilder Content(string value)
    {
        return this.Content(ActivityStream.ValueObjects.Content.From(value));
    }

    public ObjectBuilder Content(Content value)
    {
        _content = value;
        return this;
    }

    public ObjectBuilder AddTypeName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _typeNames ??= new List<TypeName>();
            _typeNames.Add(TypeName.From(name));
        }
        return this;
    }

    public ObjectBuilder AddAttachment(ObjectBuilder value)
    {
        if (value is not null)
        {
            _attachment ??= new List<ObjectBuilder>();
            _attachment.Add(value);
        }
        return this;
    }

}
