using System.Text.Json.Serialization;

namespace BienOblige.Api.Messages;

public class ActivityPublicationResponse(string correlationId, string? actionItemId = null)
{
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; } = correlationId;

    [JsonPropertyName("actionItemId")]
    public string? ActionItemId { get; set; } = actionItemId;
}
