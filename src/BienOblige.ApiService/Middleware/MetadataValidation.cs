using BienOblige.ApiService.Constants;

namespace BienOblige.ApiService.Middleware;

public static class MetadataValidation
{
    public static async Task Validate(HttpContext context, Func<Task> next)
    {
        // Provide useful error messages if metadata is not supplied

        var actorId = context.Request.Headers[Metadata.UpdatedByIdKey];
        var actorType = context.Request.Headers[Metadata.UpdatedByTypeKey];

        var results = new List<string>();
        if (string.IsNullOrWhiteSpace(actorId))
            results.Add($"A value in the header '{Metadata.UpdatedByIdKey}' must be supplied to indicate the actor making the change");
        if (string.IsNullOrWhiteSpace(actorType))
            results.Add($"A value in the header '{Metadata.UpdatedByTypeKey}' must be supplied to indicate the type of actor making the change");

        if (results.Any())
        {
            // Return 400 Bad Request with error messages if either or both headers are missing
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(string.Join(Environment.NewLine, results));
            return; // Stop processing the request
        }
        else
            await next.Invoke();
    }
}
