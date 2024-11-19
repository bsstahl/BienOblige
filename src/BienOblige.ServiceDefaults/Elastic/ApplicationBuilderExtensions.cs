namespace BienOblige.ServiceDefaults.Elastic;

public static class ApplicationBuilderExtensions
{
    const int _searchPort = 53832;

    public static IResourceBuilder<ElasticsearchResource> UseBienObligeElasticSearch(
        this IDistributedApplicationBuilder builder,
        string serviceName, ContainerLifetime? lifetime = null)
    {
        var password = builder.AddParameter("search-password", secret: true);
        return builder
            .AddElasticsearch(serviceName, password, port: _searchPort)
            .WithLifetime(lifetime ?? ContainerLifetime.Session);
    }
}
