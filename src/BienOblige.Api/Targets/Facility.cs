using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Targets;

/// <summary>
/// Represents a commercial property entity. It includes attributes relevant to commercial spaces, 
/// such as facility maintenance, leasing operations, or space utilization projects.
/// </summary>
public class Facility : IActionItemTarget
{
    /// <summary>
    /// The unique network identifier for the entity
    /// </summary>
    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    /// <summary>
    /// A human-readable name for the facility. 
    /// For US addresses, this is often the street address, city & state
    /// For named properties (ie IBM Headquarters) this is the name of the property.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The full address of the facility. 
    /// </summary>
    [JsonPropertyName("schema:address")]
    public string? Address { get; set; }

    /// <summary>
    /// The RESO Property Type of the facility.
    /// </summary>
    public string? PropertyType { get; set; }

    /// <summary>
    /// A full description of the commercial property 
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
            .AddObjectType($"schema:Accomodation")
            .AddObjectType("Object")
            .Name(this.Name)
            .Content(this.Description, this.MediaType ?? "text/plain")
            .Summary(this.Summary)
            .AddAdditionalProperty("schema:Address", this.Address)
            .AddAdditionalProperty("schema:PropertyType", this.PropertyType)
            .Build();
    }
}
