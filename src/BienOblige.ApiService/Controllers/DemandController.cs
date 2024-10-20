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

        // GET api/Demand/5
        [HttpGet("{id}")]
        public Entities.ActionItem Get(string id) => Entities.ActionItem
            .From(_demandClient.FindActionItem(NetworkIdentity.From(id)));

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
    }
}
