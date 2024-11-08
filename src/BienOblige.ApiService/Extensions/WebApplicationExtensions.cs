namespace BienOblige.ApiService.Extensions;

public static class WebApplicationExtensions
{
    public static void UseCorrelation(this IApplicationBuilder app)
    {
        app.Use(Middleware.Correlation.ValidateId);
    }

    public static void ConvertActionItemToCollection(this IApplicationBuilder app)
    {
        app.Use(Middleware.ActionItem.ConvertSingularToCollection);
    }
}
