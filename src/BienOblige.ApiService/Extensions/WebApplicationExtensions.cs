namespace BienOblige.ApiService.Extensions;

public static class WebApplicationExtensions
{
    public static void UseBearerTokenAuthentication(this IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware.BearerTokenAuthentication>();
    }
}
