namespace BienOblige.ApiService.Extensions;

public static class WebApplicationExtensions
{
    public static void UseBearerTokenAuthentication(this IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware.BearerTokenAuthentication>();
    }

    public static void UseBienObligeValidation(this IApplicationBuilder app)
    {
        app
            .UseMiddleware<Middleware.ValidateActivityCollection>()
            .UseMiddleware<Middleware.PostDataValidation>();
    }

}
