using BienOblige.Demand.Application;
using BienOblige.Demand.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandController : Controller
    {
        private readonly Client _demandClient;
        private readonly ILogger _logger;

        public DemandController(ILogger<DemandController> logger, Client demandClient)
        {
            _logger = logger;
            _demandClient = demandClient;
        }

        // POST api/<DemandController>
        [HttpPost()]
        public async Task<string> Create([FromBody] Entities.ActionItem item,
            [FromHeader(Name = "X-User-ID")] string userId,
            [FromHeader(Name = "X-Correlation-ID")] string correlationId)
        {
            // TODO: Figure out how to manage the correlation Id ideally so that it doesn't have
            // to get passed-in to the model layers since it is not needed for the
            // Application proper (just the API)

            _logger.LogInformation("Creating ActionItem with correlation ID {CorrelationId}", correlationId);

            var resultId = await _demandClient.CreateActionItem(item.AsAggregate(), NetworkIdentity.From(userId), correlationId);
            var result = resultId.Value.ToString();
            
            _logger.LogInformation("Created ActionItem with correlation ID {CorrelationId}. Result: {Result}", correlationId, result);
            
            return result;
        }

        // PATCH api/<DemandController>/5
        [HttpPatch("{id}")]
        public async Task Cancel(string id,
            [FromHeader(Name = "X-User-ID")] string userId,
            [FromHeader(Name = "X-Correlation-ID")] string correlationId)
            => await _demandClient.CancelActionItem(id, userId, correlationId);

        // TODO: Determine if Edit methods are necessary
        // it will depend on what types of use-cases require modifying
        // the underlying data that encompasses demand. There will be
        // separate methods in the Execution space that will be responsible
        // for making edits that represent activity on the ActionItem.
    }
}
