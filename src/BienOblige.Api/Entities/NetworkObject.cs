using BienOblige.Api.Builders;
using BienOblige.Api.Extensions;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class NetworkObject
{
    private readonly List<NetworkObject> _attachments = new();
    private readonly List<NetworkObject> _bcc = new();
    private readonly List<NetworkObject> _inReplyTo = new();
    private readonly List<NetworkObject> _replies = new();
    private readonly List<NetworkObject> _tag = new();
    private readonly List<NetworkObject> _location = new();
    private readonly List<Uri> _url = new();


    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    public List<KeyValuePair<string?, string>> Context { get; set; } = new();

    [JsonPropertyName("id")]
    public required Uri ObjectId { get; set; }

    [JsonPropertyName("@type")]
    public List<string> ObjectType { get; set; } = new();


    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Content { get; set; }

    [JsonPropertyName("mediaType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MediaType { get; set; }


    [JsonPropertyName("attachment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Attachments 
    { 
        get => _attachments.Any() ? _attachments : null; 
        set => _attachments.AddRange(value ?? []); 
    }

    [JsonPropertyName("attributedTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? AttributedTo { get; set; }

    [JsonPropertyName("audience")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Audience { get; set; }

    [JsonPropertyName("bcc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Bcc 
    { 
        get => _bcc.Any() ? _bcc : null; 
        set => _bcc.AddRange(value ?? []); 
    }

    [JsonPropertyName("bto")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Bto { get; set; }

    [JsonPropertyName("cc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Cc { get; set; }

    [JsonPropertyName("duration")]
    [JsonConverter(typeof(Iso8601TimespanConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimeSpan? Duration { get; set; }

    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("generator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Generator { get; set; }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Icon { get; set; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Image { get; set; }

    [JsonPropertyName("inReplyTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? InReplyTo 
    { 
        get => _inReplyTo.Any() ? _inReplyTo : null; 
        set => _inReplyTo.AddRange(value ?? []); 
    }

    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Location 
    { 
        get => _location.Any() ? _location : null; 
        set => _location.AddRange(value ?? []); 
    }

    [JsonPropertyName("preview")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Preview { get; set; }

    [JsonPropertyName("published")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? Published { get; set; }

    [JsonPropertyName("replies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Replies 
    { 
        get => _replies.Any() ? _replies : null; 
        set => _replies.AddRange(value ?? []); 
    }

    [JsonPropertyName("startTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Tags 
    { 
        get => _tag.Any() ? _tag : null; 
        set => _tag.AddRange(value ?? []); 
    }

    [JsonPropertyName("to")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? To { get; set; }

    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? LastUpdatedAt { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Uri>? Url 
    { 
        get => _url.Any() ? _url : null; 
        set => _url.AddRange(value ?? []); 
    }


    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();


    public ObjectBuilder AsObjectBuilder()
    {
        return new ObjectBuilder()
            .Id(this.ObjectId)
            .AddObjectTypes(this.ObjectType)
            .Name(this.Name)
            .Content(this.Content, this.MediaType)
            .AddAttachments(this.Attachments.AsObjectBuilders())
            .AttributedTo(this.AttributedTo?.AsObjectBuilder())
            .Audience(this.Audience?.AsObjectBuilder())
            .AddBccs(this.Bcc.AsObjectBuilders())
            .Bto(this.Bto?.AsObjectBuilder())
            .Cc(this.Cc?.AsObjectBuilder())
            .Duration(this.Duration)
            .EndTime(this.EndTime)
            .Generator(this.Generator?.AsObjectBuilder())
            .Icon(this.Icon?.AsObjectBuilder())
            .Image(this.Image?.AsObjectBuilder())
            .AddInReplyTo(this.InReplyTo.AsObjectBuilders())
            .AddLocations(this.Location?.AsObjectBuilders())
            .Preview(this.Preview?.AsObjectBuilder())
            .Published(this.Published)
            .AddReplies(this.Replies.AsObjectBuilders())
            .StartTime(this.StartTime)
            .Summary(this.Summary)
            .AddTags(this.Tags.AsObjectBuilders())
            .To(this.To?.AsObjectBuilder())
            .LastUpdatedAt(this.LastUpdatedAt)
            .AddUrls(this.Url)
            .AddAdditionalProperties(this.AdditionalProperties);
    }
}
