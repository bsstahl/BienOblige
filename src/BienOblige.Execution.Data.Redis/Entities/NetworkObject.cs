using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Redis.Entities;

public class NetworkObject
{
    private readonly List<NetworkObject> _attachments = new();
    private readonly List<NetworkObject> _bcc = new();
    private readonly List<NetworkObject> _inReplyTo = new();
    private readonly List<NetworkObject> _replies = new();
    private readonly List<NetworkObject> _tag = new();
    private readonly List<NetworkObject> _location = new();
    private readonly List<string> _url = new();


    [JsonPropertyName("@type")]
    public string[] ObjectType { get; private set; } = new[] { "Object" };

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Content { get; set; }



    // ***************************


    [JsonPropertyName("mediaType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MediaType { get; set; }


    [JsonPropertyName("attachment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Attachment
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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Duration { get; set; }

    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EndTime { get; set; }

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
    public string? Published { get; set; }

    [JsonPropertyName("replies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Replies
    {
        get => _replies.Any() ? _replies : null;
        set => _replies.AddRange(value ?? []);
    }

    [JsonPropertyName("startTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StartTime { get; set; }

    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Tag
    {
        get => _tag.Any() ? _tag : null;
        set => _tag.AddRange(value ?? []);
    }

    [JsonPropertyName("to")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? To { get; set; }

    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastUpdatedAt { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Url
    {
        get => _url.Any() ? _url : null;
        set => _url.AddRange(value ?? []);
    }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();

    public ActivityStream.Aggregates.NetworkObject AsAggregate()
    {
        return new ActivityStream.Aggregates.NetworkObject()
        {
            Id = ActivityStream.ValueObjects.NetworkIdentity.From(this.Id),
            Name = string.IsNullOrWhiteSpace(this.Name) ? null : ActivityStream.ValueObjects.Name.From(this.Name),
            Content = ActivityStream.ValueObjects.Content.From(this.Content),
            ObjectTypeName = this.ObjectType.Select(t => ActivityStream.ValueObjects.TypeName.From(t))
        };
    }

    public static NetworkObject From(ActivityStream.Aggregates.NetworkObject item)
    {
        return new NetworkObject()
        {
            Id = item.Id.Value.ToString(),
            Name = item.Name?.Value ?? string.Empty,
            Content = item.Content?.Value ?? string.Empty,
            ObjectType = item.ObjectTypeName.Select(t => t.Value).ToArray()
        };
    }

}
