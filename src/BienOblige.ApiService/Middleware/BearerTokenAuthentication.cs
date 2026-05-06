using BienOblige.ApiService.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BienOblige.ApiService.Middleware;

public class BearerTokenAuthentication
{
    const string _tokenKey = "Authorization";
    const string _bearerPrefix = "Bearer ";

    private readonly RequestDelegate _next;
    private readonly BearerTokenAuthenticationOptions _options;
    private readonly ILogger _logger;

    public BearerTokenAuthentication(RequestDelegate next, ILogger<BearerTokenAuthentication> logger, IOptions<BearerTokenAuthenticationOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (ValidateRequest(context))
        {
            await _next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Title = "Unauthorized",
                Detail = "Bearer token is missing or invalid",
                Status = 401,
                Instance = context.Request.Path
            });
        }
    }

    private bool ValidateRequest(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(_tokenKey, out var headerValue))
        {
            _logger.LogWarning("'{TokenKey}' header not supplied", _tokenKey);
            return false;
        }

        var rawValue = headerValue.ToString();
        var token = rawValue.StartsWith(_bearerPrefix, StringComparison.OrdinalIgnoreCase)
            ? rawValue[_bearerPrefix.Length..]
            : rawValue;

        var isValid = _options.ValidTokens.Any(t => StringComparer.Ordinal.Equals(t, token));
        if (!isValid)
            _logger.LogWarning("Provided bearer token is not in the list of valid tokens");

        return isValid;
    }
}
