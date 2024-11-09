using System.Text.Json;
using System.Text;
using BienOblige.ApiService.Constants;
using BienOblige.ApiService.Extensions;

namespace BienOblige.ApiService.Middleware;

public class ActionItem
{
    public static async Task ValidateIdSupplied(HttpContext context, Func<Task> next)
    {
        // Only intercept PATCH requests at the base Execution route
        if (context.Request.Path.StartsWithSegments(RoutePath.Execution)
            && context.Request.Method.Equals(HttpMethods.Patch))
        {
            string requestBody = await context.GetRequestBody();
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                // This is an Update request
                var results = new List<string>();
                using (JsonDocument doc = JsonDocument.Parse(requestBody))
                {
                    if (!doc.RootElement.TryGetProperty("id", out _))
                    {
                        // Return 400 Bad Request with error messages if the Id is missing
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("An 'id' property must be supplied for the ActionItem in the request");
                        return; // Stop processing the request
                    }
                }
            }
        }

        await next.Invoke(); // Call the next middleware in the pipeline
    }

    public static async Task ConvertSingularToCollection(HttpContext context, Func<Task> next)
    {
        // Only intercept POST requests at the base Execution route
        if (context.Request.Path.StartsWithSegments(RoutePath.Execution)
            && context.Request.Method.Equals(HttpMethods.Post))
        {
            string requestBody = await context.GetRequestBody();
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                using (JsonDocument doc = JsonDocument.Parse(requestBody))
                {
                    if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    {
                        // If it's not an array, wrap it in one
                        var wrappedJson = JsonSerializer.Serialize(new[] { doc.RootElement });

                        // Write back the modified body
                        var byteArray = Encoding.UTF8.GetBytes(wrappedJson);
                        context.Request.Body = new MemoryStream(byteArray);
                    }
                }
            }
        }

        await next.Invoke(); // Call the next middleware in the pipeline
    }
}