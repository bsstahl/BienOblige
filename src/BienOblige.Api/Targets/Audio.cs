using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Extensions;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Targets;

/// <summary>
/// Represents an audio file or track
/// </summary>
public class Audio: IActionItemTarget
{
    /// <summary>
    /// The unique network identifier for the audio file or track
    /// </summary>
    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    /// <summary>
    /// The title of the audio file or track
    /// </summary>
    [JsonPropertyName("name")]
    public required string Title { get; set; }

    /// <summary>
    /// If applicable - can include text or captions related to the audio.
    /// </summary>
    /// <remarks>When specified, the <see cref="MediaType">MediaType</see> propery should also 
    /// be defined to indicate the format of the data stored in this property.</remarks>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    /// The MIME type of the data in the Content property
    /// </summary>
    [JsonPropertyName("mediaType")]
    public string? MediaType { get; set; }

    /// <summary>
    /// Direct link(s) to the audio file(s) or related resource(s)
    /// </summary>
    [JsonPropertyName("url")]
    public List<Uri> Urls { get; set; } = new();

    /// <summary>
    /// The release date of the audio
    /// </summary>
    [JsonPropertyName("published")]
    public DateTimeOffset? PublicationDate { get; set; }

    /// <summary>
    /// A brief summary or additional information about the audio
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Description { get; set; }

    /// <summary>
    /// The length of the audio file or track
    /// </summary>
    [JsonPropertyName("duration")]
    [JsonConverter(typeof(Iso8601TimespanConverter))]
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// The artist or creator of the audio file or track
    /// </summary>
    [JsonPropertyName("attributedTo")]
    public NetworkObject? Creator { get; set; }

    /// <summary>
    /// Any relevant tags or genres associated with the audio file or track
    /// </summary>
    [JsonPropertyName("tag")]
    public List<NetworkObject> Tag { get; set; } = new();

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
            .AddObjectType(nameof(Audio))
            .Name(this.Title)
            .AddUrls(this.Urls)
            .Content(this.Content, this.MediaType)
            .Published(this.PublicationDate)
            .Summary(this.Description)
            .Duration(this.Duration)
            .AttributedTo(this.Creator?.AsObjectBuilder())
            .AddTags(this.Tag.AsObjectBuilders())
            .Build();
    }
}
