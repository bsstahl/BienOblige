using BienOblige.Api.Builders;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Place
{
    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ContextConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<KeyValuePair<string?, string>>? Context { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("@type")]
    public string ObjectType { get; set; } = "Place";

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();


    public NetworkObject AsNetworkObject()
    {
        return this.AsObjectBuilder().Build();
    }

    public ObjectBuilder AsObjectBuilder()
    {
        return new ObjectBuilder()
            .AddContext(this.Context)
            .Id(this.Id)
            .AddObjectType(this.ObjectType)
            .Name(this.Name)
            .AddAdditionalProperties(this.AdditionalProperties);
    }
}
