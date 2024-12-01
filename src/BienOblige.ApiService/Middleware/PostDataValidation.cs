using BienOblige.Api.Entities;
using BienOblige.Api.ValueObjects;
using BienOblige.ApiService.Extensions;

namespace BienOblige.ApiService.Middleware;

public class PostDataValidation
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;

    public PostDataValidation(RequestDelegate next, ILogger<BearerTokenAuthentication> logger, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { { "Method", "PostDataValidation.InvokeAsync" }}))
        {
            ArgumentNullException.ThrowIfNull(context);

            context.Request.EnableBuffering();
            bool isDirty = false;

            // Previously executed Middleware should guarantee success here
            var activities = (await context.GetActivitiesCollection() ?? Array.Empty<Activity>()).ToList();

            // If any activity has a correlation Id, they all need to match
            var cIds = activities.Select(a => a.CorrelationId).Distinct();
            var correlationIdCount = cIds.Count();
            Uri? correlationId = null;
            if (correlationIdCount > 1)
            {
                string message = "Conflicting Correlation Ids. Each item in a request must be correlated to the others in the same request.";
                _logger.LogError(message + " CorrelationIds: {CorrelationIds}", cIds);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(message + $" CorrelationIds: {cIds}");
                return; // Do not invoke the next pipeline activity
            }
            else
            {
                // Either 0 or 1 correlation Ids were supplied
                correlationId = cIds.SingleOrDefault() ?? NetworkIdentity.New().Value;
                _logger.LogInformation("Correlation Id: {CorrelationId}", correlationId);
            }

            foreach (var act in activities)
            {
                // Since we already know from above that all supplied correlation Ids are the same,
                // we can just update all those that weren't supplied by updating any that are
                // different from the first one we settled on.
                if (!correlationId.Equals(act.CorrelationId))
                {
                    act.CorrelationId = correlationId;
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                _logger.LogInformation("Correlation Ids were updated to: {CorrelationId}", correlationId);
                context.RewriteRequest(activities);
            }
            else
                _logger.LogInformation("No updates were required to Activity Correlation Ids");

            await _next.Invoke(context);
        }
    }
}
