using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Targets;

/// <summary>
/// Represents a residential property entity. It includes attributes pertinent to residential spaces, 
/// such as maintenance schedules, occupancy management, or home improvement projects. 
/// </summary>
public class Residence : IActionItemTarget
{
    /// <summary>
    /// The unique network identifier for the entity
    /// </summary>
    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    /// <summary>
    /// A human-readable name of the residence. 
    /// For US addresses, this is often the street address, city & state
    /// For named properties (ie Graceland or Hearst Castle) this is the name of the property.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The full address of the residence. 
    /// </summary>
    [JsonPropertyName("schema:address")]
    public string? Address { get; set; }

    /// <summary>
    /// A full description of the residential property 
    /// </summary>
    [JsonPropertyName("content")]
    public string? Description { get; set; }

    /// <summary>
    /// The MIME type of the data in the Content property. Usually text/plain.
    /// </summary>
    [JsonPropertyName("mediaType")]
    public string? MediaType { get; set; }

    /// <summary>
    /// A full summary of everything known about the property and the current situation. This may be AI generated.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }


    /// <summary>
    /// Converts the entity object to a generic NetworkObject format
    /// Any properties that cannot directly be applied to one of the NetworkObject properties
    /// should be added to the AdditionalProperties dictionary.
    /// </summary>
    /// <returns>An instance of a <see cref="NetworkObject"/> containing the information about the entity</returns>
    public NetworkObject AsNetworkObject()
    {
        return new ObjectBuilder()
            .Id(this.Id)
            .AddObjectType($"schema:{nameof(Residence)}")
            .AddObjectType("Object")
            .Name(this.Name)
            .Summary(this.Summary)
            .Content(this.Description, this.MediaType ?? "text/plain")
            .AddAdditionalProperty("schema:Address", this.Address)
            .Build();
    }
}
