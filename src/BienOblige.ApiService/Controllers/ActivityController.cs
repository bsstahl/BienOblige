using BienOblige.Api.Entities;
using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Application;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/v1/[controller]/inbox")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly Client _executionClient;
        private readonly ILogger _logger;

        public ActivityController(ILogger<ActivityController> logger, Client executionClient)
        {
            _logger = logger;
            _executionClient = executionClient;
        }

        public IActionResult Index()
        {
            return new JsonResult(Array.Empty<string>());
        }

        // POST api/v1/Activity
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Activity activity)
        {
            //_logger.LogInformation("Creating ActionItem for request with correlation ID {CorrelationId}", correlationId);

            var resultId = await _executionClient.PublishActivityCommand(activity.AsAggregate());
            var result = new Api.Messages.ActivityPublicationResponse(resultId.Value.ToString(), activity.ActionItem?.Id);

            // _logger.LogInformation("Created {Count} ActionItem(s) for request with correlation ID {@CorrelationId}. Result: {@Result}", resultIds.Count(), correlationId, result);

            return new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };
        }

    }
}
