using BienOblige.ActivityStream.ValueObjects;
using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Search.Application
{
    public class Client
    {
        private readonly ILogger _logger;
        private readonly ElasticsearchClient _searchClient;

        public Client(ILogger<Client> logger, ElasticsearchClient searchClient)
        {
            _logger = logger;
            _searchClient = searchClient;
        }

        public async Task<NetworkObject?> Get(NetworkIdentity id)
        {
            var result = await _searchClient.GetAsync<NetworkObject>(id.Value.ToString());
            if (!result.IsSuccess())
                _logger.LogWarning("Unable to find ActionItem with ID {ActionItemId}", id);
            return result.Source;
        }
    }
}
