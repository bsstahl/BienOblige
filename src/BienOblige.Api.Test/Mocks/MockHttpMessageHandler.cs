using BienOblige.Api.Entities;
using BienOblige.ApiClient;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Test.Mocks;

[ExcludeFromCodeCoverage]
public class MockHttpMessageHandler : Mock<HttpMessageHandler>
{
    private readonly List<HttpContent> _requestContent = new();
    public IEnumerable<HttpContent> RequestContent => _requestContent;

    public MockHttpMessageHandler()
    {
        this.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
            {
                try
                {
                    // If the request is for a singular activity, it will be wrapped in an array
                    var requestContentTask = request.Content?.ReadAsStringAsync() ?? Task.FromResult("[]");
                    requestContentTask.Wait();
                    var requestContent = requestContentTask.Result;

                    var activity = JsonSerializer.Deserialize<Activity>(requestContent);
                    _requestContent.Add(new StringContent($"[{requestContent}]"));
                }
                catch (Exception)
                {
                    _requestContent.Add(request.Content);
                }
            })
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
            {
                var requestContent = request.Content?.ReadAsStringAsync()?.Result ?? "{}";
                var activities = JsonSerializer.Deserialize<IEnumerable<Activity>>(requestContent)
                    ?? Array.Empty<Activity>();

                var publicationResults = new List<PublicationResult>();
                foreach (var activity in activities)
                    publicationResults.Add(GetPublicationResponse(activity));

                var activityContent = new StringContent(JsonSerializer
                    .Serialize(publicationResults));

                return new HttpResponseMessage()
                {
                    StatusCode = GetOverallStatus(publicationResults),
                    Content = activityContent
                };
            });

    }

    private static HttpStatusCode GetOverallStatus(IEnumerable<PublicationResult> publicationResults)
    {
        return publicationResults.All(r => r.SuccessfullyPublished)
            ? HttpStatusCode.Accepted
            : publicationResults.All(r => !r.SuccessfullyPublished)
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.MultiStatus;
    }

    private static PublicationResult GetPublicationResponse(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity, nameof(activity));

        var actionItem = activity?.ActionItem ?? throw new ArgumentException("Invalid ActionItem");
        var shouldFail = (actionItem?.AdditionalProperties.TryGetValue("shouldFail", out var shouldFailValue) ?? false)
            ? bool.TryParse(shouldFailValue.ToString(), out var shouldFailParsed) ? shouldFailParsed : false
            : false;
        var statusCode = shouldFail ? HttpStatusCode.BadRequest : HttpStatusCode.Accepted;

        return shouldFail
            ? new PublicationResult(activity, ["An error has occurred"])
            : new PublicationResult(activity);
    }

}
