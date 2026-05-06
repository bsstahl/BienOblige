using BienOblige.ApiService.Configuration;

namespace BienOblige.ApiService.Extensions;

public static class WebApplicationExtensions
{
    public static void UseBearerTokenAuthentication(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var options = config.GetSection(BearerTokenAuthenticationOptions.SectionName)
            .Get<BearerTokenAuthenticationOptions>();

        if (options?.ValidTokens?.Count > 0)
            app.UseMiddleware<Middleware.BearerTokenAuthentication>();
    }

    public static void UseBienObligeValidation(this IApplicationBuilder app)
    {
        app
            .UseMiddleware<Middleware.ValidateActivityCollection>()
            .UseMiddleware<Middleware.PostDataValidation>();
    }

}
