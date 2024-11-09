namespace BienOblige.ApiService.Extensions;

public static class WebApplicationExtensions
{
    public static void UseCorrelation(this IApplicationBuilder app)
    {
        app.Use(Middleware.Correlation.ValidateId);
    }

    public static void ValidateActionItem(this IApplicationBuilder app)
    {
        app.Use(Middleware.ActionItem.ConvertSingularToCollection);
        app.Use(Middleware.ActionItem.ValidateIdSupplied);
    }

    public static void ValidateMetadata(this IApplicationBuilder app)
    {
        app.Use(Middleware.MetadataValidation.Validate);
    }
}
