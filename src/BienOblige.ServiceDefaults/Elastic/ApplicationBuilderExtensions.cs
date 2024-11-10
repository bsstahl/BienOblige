namespace BienOblige.ServiceDefaults.Elastic;

public static class ApplicationBuilderExtensions
{
    public static IResourceBuilder<ElasticsearchResource> UseBienObligeElasticSearch(
    this IDistributedApplicationBuilder builder,
    string serviceName)
    {
        // TODO: Add index creation here if needed
        return builder
            .AddElasticsearch(serviceName);
   
            //.WithHealthCheck([
            //    Topics.CommandChannelName,
            //    Topics.ActionItemsPublicChannelName,
            //    Topics.CompliancePublicChannelName
            //])
    }
}
