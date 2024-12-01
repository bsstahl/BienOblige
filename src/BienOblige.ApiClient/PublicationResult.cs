using BienOblige.Api.Entities;
using System.Text.Json.Serialization;

namespace BienOblige.ApiClient;

public class PublicationResult
{
    [JsonPropertyName("activity")]
    public Activity Activity { get; set; }

    [JsonPropertyName("successfullyPublished")]
    public bool SuccessfullyPublished { get; set; }

    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    /// <summary>
    /// Used by the JSON deserializer
    /// </summary>
    [JsonConstructor]
    public PublicationResult(Activity activity, bool successfullyPublished, IEnumerable<string> errors)
    { 
        this.Activity = activity;
        this.SuccessfullyPublished = successfullyPublished;
        this.Errors = errors;
    }

    /// <summary>
    /// Used when the publication was successful
    /// </summary>
    /// <param name="activity">The <see cref="Activity"/> that was published</param>
    public PublicationResult(Activity activity)
    {
        this.Activity = activity;
        this.SuccessfullyPublished = true;
        this.Errors = Enumerable.Empty<string>();
    }

    /// <summary>
    /// Used when the publication was NOT successful
    /// </summary>
    /// <param name="activity">The <see cref="Activity"/> that failed to publish</param>
    /// <param name="errors">The errors that occurred during publication</param>
    public PublicationResult(Activity activity, IEnumerable<string> errors)
    {
        this.Activity = activity;
        this.SuccessfullyPublished = false;
        this.Errors = errors;
    }
}
