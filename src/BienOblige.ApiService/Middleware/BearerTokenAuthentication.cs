namespace BienOblige.ApiService.Middleware;

public class BearerTokenAuthentication
{
    const string _tokenKey = "Authorization";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;

    public BearerTokenAuthentication(RequestDelegate next, ILogger<BearerTokenAuthentication> logger, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (await ValidateRequest(context))
        {
            await _next.Invoke(context);
        }
        else
        {
            var response = context.Response;
            response.StatusCode = 401;
            await response.WriteAsync("Unauthorized");
        }
    }

    private Task<bool> ValidateRequest(HttpContext context)
    {
        _logger.LogDebug("Config: {@Config}", _config);

        bool isValid = false;
        if (context.Request.Headers.TryGetValue(_tokenKey, out var token))
        {
            // TODO: Compare token to known acceptable values
            _logger.LogWarning("Bearer token authentication skipped (not implemented)");
            isValid = false; // should be set to true only if the token is valid 
        }
        else
        {
            _logger.LogWarning($"'{_tokenKey}' header not supplied");
            isValid = false;
        }

        return Task.FromResult(true); // TODO: Return the value of isValid
    }
}
