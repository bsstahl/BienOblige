using BienOblige.Api.Entities;

namespace BienOblige.ApiClient;

public class PublicationResult
{
    public Activity Activity { get; set; }
    public bool SuccessfullyPublished { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public PublicationResult(Activity activity, bool successfullyPublished, IEnumerable<string> errors = null)
    {
        this.Activity = activity;
        this.SuccessfullyPublished = successfullyPublished;
        this.Errors = errors ?? Enumerable.Empty<string>();
    }
}
