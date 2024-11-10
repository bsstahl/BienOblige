namespace BienOblige.ServiceDefaults.Elastic;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<ElasticsearchResource> UseBienObligeElasticSearch(
        this IDistributedApplicationBuilder builder,
        string serviceName)
    {
        var password = builder.AddParameter("search-password", secret: true);

        // TODO: Add index creation here if needed
        return builder
            .AddElasticsearch(serviceName, password);
   
            //.WithHealthCheck([
            //    Topics.CommandChannelName,
            //    Topics.ActionItemsPublicChannelName,
            //    Topics.CompliancePublicChannelName
            //])
    }
}
