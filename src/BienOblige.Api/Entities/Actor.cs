using BienOblige.Api.Builders;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Entities;

public class Actor
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("@type")]
    public required string ActorType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }


    public NetworkObject AsNetworkObject()
    {
        return this.AsObjectBuilder().Build();
    }

    public ObjectBuilder AsObjectBuilder()
    {
        return new ObjectBuilder()
            .Id(this.Id)
            .AddObjectType(this.ActorType)
            .Name(this.Name);
    }
}
