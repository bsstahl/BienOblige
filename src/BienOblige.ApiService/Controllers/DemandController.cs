using BienOblige.Demand.Application;
using BienOblige.Demand.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandController : ControllerBase
    {
        private readonly Client _demandClient;

        public DemandController(Client demandClient)
        {
            _demandClient = demandClient;
        }

        // POST api/<DemandController>
        [HttpPost]
        public string Create([FromBody] Entities.ActionItem item, string userId) 
            => _demandClient.CreateActionItem(
                    item.AsAggregate(),
                    NetworkIdentity.From(userId))
                    .Value.ToString();

        // PATCH api/<DemandController>/5
        [HttpPatch("{id}")]
        public void Cancel(string id, string userId)
            => _demandClient.CancelActionItem(id, userId);

        // TODO: Determine if Edit methods are necessary
        // it will depend on what types of use-cases require modifying
        // the underlying data that encompasses demand. There will be
        // separate methods in the Execution space that will be responsible
        // for making edits that represent activity on the ActionItem.
    }
}
