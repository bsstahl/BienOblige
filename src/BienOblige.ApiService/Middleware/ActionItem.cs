using System.Text.Json;
using System.Text;

namespace BienOblige.ApiService.Middleware;

public class ActionItem
{
    const string _routePath = "/api/Execution";

    public static async Task ConvertSingularToCollection(HttpContext context, Func<Task> next)
    {
        // Only intercept POST requests at a specific route
        if (context.Request.Method == HttpMethods.Post && context.Request.Path.StartsWithSegments(_routePath))
        {
            // Enable buffering so that we can read the request body multiple times
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset position for downstream middleware

                if (!string.IsNullOrWhiteSpace(body))
                {
                    using (JsonDocument doc = JsonDocument.Parse(body))
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
        }

        await next.Invoke(); // Call the next middleware in the pipeline
    }
}