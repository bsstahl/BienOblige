// using BienOblige.Search.Aggregates;
using BienOblige.Search.Application.Interfaces;
using BienOblige.ActivityStream.ValueObjects;
using Elastic.Clients.Elasticsearch;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Search.Data.Elastic;

public class ActionItemReadRepository : IFindActionItems
{
    ElasticsearchClient _client;

    public ActionItemReadRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public Task<IEnumerable<NetworkObject>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<NetworkObject>> GetByTarget(NetworkIdentity targetId, string targetType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<NetworkObject>> GetGraph(NetworkIdentity parentId)
    {
        throw new NotImplementedException();
    }

    //public async Task<bool> Exists(NetworkIdentity id)
    //{
    //    var searchId = id.Value.ToString();
    //    var existsResponse = await _client.ExistsAsync<ActionItem>(searchId, 
    //        d => d.Index(Constants.Indexes.ActionItemState));
    //    return existsResponse.Exists;
    //}

    //public Task<ActionItem?> Get(NetworkIdentity id)
    //{
    //    throw new NotImplementedException();
    //}

}
