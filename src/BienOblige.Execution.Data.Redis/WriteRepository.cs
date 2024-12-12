using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Redis.Entities;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BienOblige.Execution.Data.Redis;

public class WriteRepository : IUpdateActionItems
{
    private readonly ILogger _logger;
    private readonly IConnectionMultiplexer _mux;
    private readonly IDatabase _db;

    public WriteRepository(ILogger<WriteRepository> logger, IConnectionMultiplexer mux)
    {
        _logger = logger;
        _mux = mux; // TODO: Do I need to hold this reference
        _db = mux.GetDatabase(0);
    }

    public async Task<NetworkIdentity> Update(ActivityStream.Aggregates.NetworkObject changes, ActivityStream.Aggregates.Actor actor, string correlationId)
    {
        changes.LastUpdatedAt = DateTimeOffset.UtcNow;
        // changes.LastUpdatedBy = actor; TODO: Revisit this

        var key = changes.Id.Value;
        var item = NetworkObject.From(changes);

        var result = await _db.StringSetAsync(changes.Id.ToString(), item.ToString());

        if (!result)
        {
            _logger.LogError("Failed to update {@ActionItem}", changes);
            throw new Exception("Failed to update Object");
        }

        return changes.Id;
    }
}
