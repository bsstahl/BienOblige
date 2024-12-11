using BienOblige.Api.Entities;

namespace BienOblige.Api.Interfaces
{
    public interface INetworkObject
    {
        Dictionary<string, object> AdditionalProperties { get; set; }
        List<NetworkObject>? Attachment { get; set; }
        NetworkObject? AttributedTo { get; set; }
        NetworkObject? Audience { get; set; }
        List<NetworkObject>? Bcc { get; set; }
        NetworkObject? Bto { get; set; }
        NetworkObject? Cc { get; set; }
        string? Content { get; set; }
        List<KeyValuePair<string?, string>>? Context { get; set; }
        TimeSpan? Duration { get; set; }
        DateTimeOffset? EndTime { get; set; }
        NetworkObject? Generator { get; set; }
        NetworkObject? Icon { get; set; }
        Uri Id { get; set; }
        NetworkObject? Image { get; set; }
        List<NetworkObject>? InReplyTo { get; set; }
        DateTimeOffset? LastUpdatedAt { get; set; }
        List<NetworkObject>? Location { get; set; }
        string? MediaType { get; set; }
        string? Name { get; set; }
        List<string> ObjectType { get; set; }
        NetworkObject? Preview { get; set; }
        DateTimeOffset? Published { get; set; }
        List<NetworkObject>? Replies { get; set; }
        DateTimeOffset? StartTime { get; set; }
        string? Summary { get; set; }
        List<NetworkObject>? Tag { get; set; }
        NetworkObject? To { get; set; }
        List<Uri>? Url { get; set; }

        public abstract NetworkObject AsNetworkObject();
    }
}