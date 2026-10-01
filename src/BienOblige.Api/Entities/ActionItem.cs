using BienOblige.Api.Converters;
using BienOblige.Api.Enumerations;
using BienOblige.Api.Interfaces;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class ActionItem : INetworkObject
{
    public static string[] DefaultObjectTypeName = new string[] { "bienoblige:ActionItem", "Object" };

    private readonly List<NetworkObject> _attachments = new();
    private readonly List<NetworkObject> _bcc = new();
    private readonly List<NetworkObject> _inReplyTo = new();
    private readonly List<NetworkObject> _replies = new();
    private readonly List<NetworkObject> _tag = new();
    private readonly List<NetworkObject> _location = new();
    private readonly List<Uri> _url = new();

    [JsonPropertyName("bienoblige:target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Target { get; set; }

    [JsonPropertyName("bienoblige:parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Parent { get; set; }

    [JsonPropertyName("bienoblige:completionMethods")]
    [JsonConverter(typeof(EnumListConverter<CompletionMethod>))]
    public List<CompletionMethod> CompletionMethods { get; set; } = new List<CompletionMethod>();

    [JsonPropertyName("bienoblige:prerequisites")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Prerequisites { get; set; }

    #region NetworkObject Properties

    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<KeyValuePair<string?, string>>? Context { get; set; }

    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    [JsonPropertyName("@type")]
    public List<string> ObjectType { get; set; } = DefaultObjectTypeName.ToList();


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
    [JsonConverter(typeof(Iso8601TimespanConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimeSpan? Duration { get; set; }

    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? EndTime { get; set; }

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

    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? LastUpdatedAt { get; set; }

    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; }

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

    [JsonPropertyName("generator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? Generator { get; set; }

    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<NetworkObject>? Location { get; set; }

    [JsonPropertyName("tag")]
    public List<NetworkObject>? Tag { get; set; }

    [JsonPropertyName("to")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkObject? To { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Uri>? Url
    {
        get => _url.Any() ? _url : null;
        set => _url.AddRange(value ?? []);
    }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();

    #endregion

    public virtual NetworkObject AsNetworkObject()
    {
        var json = JsonSerializer.Serialize(this, this.GetType());
        return JsonSerializer.Deserialize<NetworkObject>(json)
            ?? throw new InvalidOperationException("Unable to convert into a NetworkObject");
    }
}
