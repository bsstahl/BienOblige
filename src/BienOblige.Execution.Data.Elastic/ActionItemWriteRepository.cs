using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Elastic.Extensions;
using BienOblige.ValueObjects;
using Elastic.Clients.Elasticsearch;

namespace BienOblige.Execution.Data.Elastic;

public class ActionItemWriteRepository : IUpdateActionItems
{
    ElasticsearchClient _client;

    public ActionItemWriteRepository(ElasticsearchClient client)
    {
        _client = client;
        _client.CreateIndexIfNotExist(Constants.Indexes.ActionItemState);
    }

    public async Task<NetworkIdentity> Update(ActionItem changes, Actor actor, string correlationId)
    {
        var token = new CancellationTokenSource().Token;
        var result = await _client.IndexAsync<ActionItem>(changes, Constants.Indexes.ActionItemState, changes.Id.Value.ToString(), token);
        return changes.Id;
    }

}
