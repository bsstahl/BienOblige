using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Targets;

/// <summary>
/// A motor vehicle used primarily for transportation (as opposed to one used primarily for hauling goods)
/// </summary>
public class Car : IActionItemTarget
{
    /// <summary>
    /// The unique network identifier for the audio file or track
    /// </summary>
    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    /// <summary>
    /// The name of the car. Usually the year make and model.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// A full description of the car including Year, Make, Model, Trim, 
    /// Engine Type and any other relevant information.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Description { get; set; }

    /// <summary>
    /// The MIME type of the data in the Content property. Usually text/plain.
    /// </summary>
    [JsonPropertyName("mediaType")]
    public string? MediaType { get; set; }

    /// <summary>
    /// A full summary of everything known about the car and the current situation. This may be AI generated.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    /// <summary>
    /// The Vehicle Identification Number (VIN) of the car.
    /// </summary>
    [JsonPropertyName("schema:vehicleIdentificationNumber")]
    public string? Vin { get; set; }

    /// <summary>
    /// Converts the Car object to a generic NetworkObject format
    /// Any properties that cannot directly be applied to one of the NetworkObject properties
    /// should be added to the AdditionalProperties dictionary.
    /// </summary>
    /// <returns>An instance of a <see cref="NetworkObject"/> containing the information about the Car</returns>
    public NetworkObject AsNetworkObject()
    {
        return new ObjectBuilder()
            .Id(this.Id)
            .AddObjectType($"schema:{nameof(Car)}")
            .AddObjectType("Object")
            .Name(this.Name)
            .Content(this.Description, this.MediaType ?? "text/plain")
            .Summary(this.Summary)
            .AddAdditionalProperty("schema:vehicleIdentificationNumber", this.Vin)
            .Build();
    }
}
