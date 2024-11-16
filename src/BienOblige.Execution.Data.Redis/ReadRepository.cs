using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Data.Redis.Entities;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace BienOblige.Execution.Data.Redis
{
    public class ReadRepository : IGetActionItems
    {
        private readonly IConnectionMultiplexer _mux;
        private readonly IDatabase _db;
        private readonly ILogger _logger;

        public ReadRepository(ILogger<ReadRepository> logger, IConnectionMultiplexer mux)
        {
            _logger = logger;
            _mux = mux; // TODO: Do I need to persist this reference?
            _db = mux.GetDatabase(0);
        }

        public async Task<bool> Exists(NetworkIdentity id)
        {
            return (await _db.StringGetAsync(id.ToString())).HasValue;
        }

        public async Task<Aggregates.ActionItem?> Get(NetworkIdentity id)
        {
            var result = await _db.StringGetAsync(id.ToString());
            return result.HasValue 
                ? new ActionItem(JsonDocument.Parse(result.ToString()).RootElement)?.AsAggregate()
                : null;
        }
    }
}
