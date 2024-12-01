using BienOblige.Api.Entities;
using BienOblige.ApiService.Extensions;
using System.Text.Json;

namespace BienOblige.ApiService.Middleware;

public class ValidateActivityCollection
{
    // const string _isSingularActivityKey = "isSingularActivity";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;

    public ValidateActivityCollection(RequestDelegate next, ILogger<ValidateActivityCollection> logger, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { { "Method", "ValidateActivityCollection.InvokeAsync" } }))
        {
            ArgumentNullException.ThrowIfNull(context);

            context.Request.EnableBuffering();
            bool isDirty = false;
            List<Activity>? activities = null;

            Activity? activity = await context.GetSingularActivity();
            if (activity is not null)
            {
                activities = [activity];
                // context.Items[_isSingularActivityKey] = true;
                isDirty = true;
                _logger.LogInformation("Converted singlular Activity to a collection: {ActivityCollection}", activities);
            }
            else // Not a singular activity
            {
                activities = (await context.GetActivitiesCollection())?.ToList();
                if (activities is not null)
                {
                    // context.Items[_isSingularActivityKey] = false;
                    _logger.LogTrace("{ActivityCount} Activities Received", activities.Count());
                }
                else // Can't deserialize the request to an activity collection either
                {
                    string body = await context.GetRequestBody();
                    string message = "Invalid request: Unable to deserialize payload to an Activity or collection of Activities";
                    _logger.LogError(message + " Payload: {Payload}", body);
                    await context.WriteRequestErrorResponse(message, body);
                    return; // Do not invoke the next pipeline activity
                }
            }

            if (isDirty)
            {
                _logger.LogInformation("Request body rewritten: {Activities}", JsonSerializer.Serialize(activities));
                context.RewriteRequest(activities);
            }

            await _next.Invoke(context);

            //// If this was a singular Activity request (the AS2 standard), convert back to a singular response
            //// We need to do so only if context.Items[_isSingluarActivityKey] is true
            //if (context.Items.TryGetValue(_isSingularActivityKey, out var isSingularActivity)
            //    && (bool)isSingularActivity)
            //{
            //    var publicationResults = await context.GetPublicationResults();

            //    // It should not be possible to have more than one element in this collection
            //    var publicationResult = publicationResults?.Single() ?? throw new InvalidOperationException();
            //    context.RewriteResponse(publicationResult);
            //}
        }
    }
}
