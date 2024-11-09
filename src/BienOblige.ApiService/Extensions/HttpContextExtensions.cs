using System.Text;

namespace BienOblige.ApiService.Extensions;

public static class HttpContextExtensions
{
    public static async Task<string> GetRequestBody(this HttpContext context)
    {
        string result = string.Empty;

        context.Request.EnableBuffering(); // Enable buffering so that we can read the request body multiple times
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            result = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // Reset position for downstream middleware
        }

        return result;
    }

}
