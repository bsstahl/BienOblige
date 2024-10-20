namespace BienOblige.ApiService.Middleware;

public static class Correlation
{
    const string correlationIdKey = "X-Correlation-ID";

    public static async Task ValidateId(HttpContext context, Func<Task> next)
    {
        var id = context.Request.Headers[correlationIdKey];
        if (string.IsNullOrWhiteSpace(id))
        {
            id = context.TraceIdentifier ?? Guid.NewGuid().ToString();
            context.Request.Headers.Append(correlationIdKey, id);
        }
        await next.Invoke();
    }
}
