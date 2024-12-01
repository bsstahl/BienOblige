using BienOblige.Api.Entities;
using BienOblige.ApiClient;
using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        // Singular requests are converted to a collection type in middleware
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] IEnumerable<Activity> activities)
        {
            _logger.LogInformation("Processing request on ActivityController");

            var correlationId = activities.First().CorrelationId;
            _logger.LogInformation("Attempting to create Activities for the request with correlation ID {CorrelationId}", correlationId);

            var results = new List<PublicationResult>();
            foreach (var activity in activities)
            {
                try
                {
                    var resultId = await _executionClient.PublishActivityCommand(activity.AsAggregate());
                    activity.ActionItem.Id = resultId.Value.ToString();
                    results.Add(new PublicationResult(activity));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating Activity for request with correlation ID {CorrelationId}. Error: {ErrorMessage}.", correlationId, ex.Message);
                    results.Add(new PublicationResult(activity, [ex.Message]));
                }
            }

            var statusCode = (results.All(r => r.SuccessfullyPublished))
                ? HttpStatusCode.Accepted // All requests succeeded
                : (results.All(r => !r.SuccessfullyPublished))
                    ? HttpStatusCode.BadRequest // All requests failed
                    : HttpStatusCode.MultiStatus; // Some requests failed

            var successCount = results.Count(r => r.SuccessfullyPublished);
            var failCount = results.Count(r => !r.SuccessfullyPublished);

            if (statusCode.Equals(HttpStatusCode.Accepted))
                _logger.LogInformation("Successfully Created {Count} Activities for request with correlation ID {CorrelationId}. Result: {@Result}", successCount, correlationId, results);
            else if (statusCode.Equals(HttpStatusCode.BadRequest))
                _logger.LogWarning("Failed to create {Count} Activities for request with correlation ID {CorrelationId}. Result: {@Result}", failCount, correlationId, results);
            else
                _logger.LogWarning("Partial Success: Created {SuccessCount} but Failed to create {FailCount} Activities for request with correlation ID {CorrelationId}. Result: {@Result}", successCount, failCount, correlationId, results);

            return new JsonResult(results)
            {
                StatusCode = (int)statusCode
            };
        }

    }
}
