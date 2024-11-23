using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Interfaces;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Targets;

/// <summary>
/// Represents a text-based content or informational entity. It could be a piece of written content, 
/// documentation, or any form of structured information that can be acted upon or referenced. 
/// </summary>
public class Article : IActionItemTarget
{
    /// <summary>
    /// The unique network identifier for the article
    /// </summary>
    [JsonPropertyName("id")]
    public required Uri Id { get; set; }

    /// <summary>
    /// The title of the article
    /// </summary>
    [JsonPropertyName("name")]
    public required string Title { get; set; }

    /// <summary>
    /// The full text of the article or other content relevant to the article.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    /// The MIME type of the data in the Content property
    /// </summary>
    [JsonPropertyName("mediaType")]
    public string? MediaType { get; set; }

    /// <summary>
    /// The date the article was published. 
    /// If it has not yet been published, this property may be null, or may contain the planned future publication date.
    /// </summary>
    [JsonPropertyName("published")]
    public DateTimeOffset? PublicationDate { get; set; }

    /// <summary>
    /// A full summary of everything known about the article and the current situation. This may be AI generated.
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
            .AddObjectType(nameof(Article))
            .Name(this.Title)
            .Content(this.Content, this.MediaType)
            .Published(this.PublicationDate)
            .Summary(this.Summary)
            .Build();
    }
}
