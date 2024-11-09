using System.Text.Json.Serialization;

namespace BienOblige.ApiService.Entities;

public class CreateResponse(IEnumerable<string> ids)
{
    [JsonPropertyName("Ids")]
    public IEnumerable<string> Ids { get; set; } = ids;
}
