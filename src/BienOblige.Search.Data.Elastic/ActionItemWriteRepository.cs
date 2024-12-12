// using BienOblige.Search.Aggregates;
using BienOblige.Search.Application.Interfaces;
using BienOblige.Search.Data.Elastic.Extensions;
using BienOblige.ActivityStream.ValueObjects;
using Elastic.Clients.Elasticsearch;
using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Search.Data.Elastic;

public class ActionItemWriteRepository : IUpdateActionItems
{
    ElasticsearchClient _client;

    public ActionItemWriteRepository(ElasticsearchClient client)
    {
        _client = client;
        _client.CreateIndexIfNotExist(Constants.Indexes.ActionItemState);
    }

    public async Task<NetworkIdentity> Update(NetworkObject changes, Actor actor, string correlationId)
    {
        var token = new CancellationTokenSource().Token;
        var result = await _client.IndexAsync<NetworkObject>(changes, Constants.Indexes.ActionItemState, changes.Id.Value.ToString(), token);
        return changes.Id;
    }

}
