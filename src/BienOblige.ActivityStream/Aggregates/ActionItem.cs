using BienOblige.ActivityStream.Collections;
using BienOblige.ActivityStream.ValueObjects;
using System.Text.Json.Serialization;

namespace BienOblige.ActivityStream.Aggregates;

public class ActionItem : NetworkObject
{
    // TODO: Convert NetworkObjects to more specific types as appropriate

    [JsonPropertyName("bienoblige:exceptions")]
    public ExceptionCollection Exceptions { get; set; } = new();

    [JsonPropertyName("bienoblige:executorRequirements")]
    public RequirementsCollection ExecutorRequirements { get; set; } = new();

    [JsonPropertyName("bienoblige:parent")]
    public NetworkIdentity? Parent { get; set; }

    [JsonPropertyName("bienoblige:origin")]
    public NetworkObject? Origin { get; set; }

    [JsonPropertyName("bienoblige:priority")]
    public NetworkObject? Priority { get; set; }

    [JsonPropertyName("bienoblige:status")]
    public NetworkObject? Status { get; set; }

    [JsonPropertyName("bienoblige:effort")]
    public NetworkObject? Effort { get; set; }

    [JsonPropertyName("bienoblige:target")]
    public NetworkObject? Target { get; set; }

    [JsonPropertyName("bienoblige:updatedBy")]
    public Actor? LastUpdatedBy { get; set; }

    public ActionItem(Name name, Content content)
        : this(NetworkIdentity.New(), name, content)
    { }

    public ActionItem(NetworkIdentity id, Name name, Content content)
        :base(id, TypeName.From(typeof(ActionItem)))
    {
        base.Id = id;
        base.Name = name;
        base.Content = content;
    }

}
