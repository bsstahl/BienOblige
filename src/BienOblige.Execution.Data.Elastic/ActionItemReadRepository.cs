using BienOblige.Execution.Aggregates;
using BienOblige.ActivityStream.ValueObjects;
using Elastic.Clients.Elasticsearch;

namespace BienOblige.Execution.Data.Elastic;

public class ActionItemReadRepository : IFindActionItems
{
    ElasticsearchClient _client;

    public ActionItemReadRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<bool> Exists(NetworkIdentity id)
    {
        var searchId = id.Value.ToString();
        var existsResponse = await _client.ExistsAsync<ActionItem>(searchId, 
            d => d.Index(Constants.Indexes.ActionItemState));
        return existsResponse.Exists;
    }

    public Task<ActionItem?> Get(NetworkIdentity id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ActionItem>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ActionItem>> GetGraph(NetworkIdentity parentId)
    {
        throw new NotImplementedException();
    }
}
