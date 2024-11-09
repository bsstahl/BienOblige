using System.Text.Json.Serialization;

namespace BienOblige.ApiService.Entities;

public class UpdateResponse(string id)
{
    [JsonPropertyName("Id")]
    public string Id { get; set; } = id;
}
