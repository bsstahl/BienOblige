using Elastic.Clients.Elasticsearch;

namespace BienOblige.Execution.Data.Elastic.Extensions;

internal static class ElasticsearchClientExtensions
{
    // Note: this method is made synchronous since it is called from the constructor
    internal static void CreateIndexIfNotExist(this ElasticsearchClient client, string indexName)
    {
        var existsResponseTask = client.Indices.ExistsAsync(Constants.Indexes.ActionItemState);
        existsResponseTask.Wait();
        var existsResponse = existsResponseTask.Result;

        if (!existsResponse.Exists)
        {
            // Index does not exist, create it
            // TODO: Make the parameters configurable
            var createIndexResponseTask = client.Indices
                .CreateAsync(indexName, c => c.Settings(s => s.NumberOfShards(1).NumberOfReplicas(1)));
            createIndexResponseTask.Wait();
        }
    }
}
