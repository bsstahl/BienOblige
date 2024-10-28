using System.Text.Json.Serialization;

namespace BienOblige.ApiService.Entities;

public class CreateResponse(string id)
{
    [JsonPropertyName("Id")]
    public string Id { get; set; } = id;
}
