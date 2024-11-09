using BienOblige.ApiService.Entities;
using BienOblige.ApiService.Extensions;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application;
using BienOblige.ValueObjects;
using BienOblige.ApiService.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly Client _executionClient;
        private readonly ILogger _logger;

        public ExecutionController(ILogger<ExecutionController> logger, Client executionClient)
        {
            _logger = logger;
            _executionClient = executionClient;
        }

        // POST api/<ExecutionController>
        // Middleware converts all singular ActionItem requests to IEnumerable<ActionItem> requests
        // Middleware also validates that metadata headers are present as needed
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] IEnumerable<Entities.ActionItem> items,
            [FromHeader(Name = Metadata.UpdatedByIdKey)] string updatedById,
            [FromHeader(Name = Metadata.UpdatedByTypeKey)] string updatedByType,
            [FromHeader(Name = Metadata.CorrelationIdKey)] string correlationId)
        {
            //_logger.LogInformation("Creating ActionItem for request with correlation ID {CorrelationId}", correlationId);
            
            // Add Ids to any ActionItem that doesn't have one
            items.ToList().ForEach(item => item.Id ??= NetworkIdentity.New().Value.ToString());

            var updatingActor = Actor.From(updatedById, updatedByType);

            var resultIds = await _executionClient.CreateActionItem(
                items.AsAggregates(),
                updatingActor,
                correlationId);

            var resultIdValues = resultIds.Select(t => t.Value.ToString());
            var result = new CreateResponse(resultIdValues);

            // _logger.LogInformation("Created {Count} ActionItem(s) for request with correlation ID {@CorrelationId}. Result: {@Result}", resultIds.Count(), correlationId, result);

            Response.Headers[Metadata.CorrelationIdKey] = correlationId;
            return new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };
        }

        // PATCH api/<ExecutionController>
        // Middleware validates that the supplied ActionItem includes its Id
        // Middleware also validates that metadata headers are present as needed
        [HttpPatch()]
        public async Task<IActionResult> Update(
            [FromBody] Entities.ActionItem item,
            [FromHeader(Name = Metadata.UpdatedByIdKey)] string updatedById,
            [FromHeader(Name = Metadata.UpdatedByTypeKey)] string updatedByType,
            [FromHeader(Name = Metadata.CorrelationIdKey)] string correlationId)
        {
            //_logger.LogInformation("Updating ActionItem for with CorrelationId {CorrelationId}", correlationId);

            var updatingActor = Actor.From(updatedById, updatedByType);

            var resultId = await _executionClient.UpdateActionItem(
                item.AsAggregate(),
                updatingActor, correlationId);

            var result = new UpdateResponse(resultId.Value.ToString());

            //_logger.LogInformation("Updating ActionItem for request with CorrelationId {CorrelationId}. Result: {Result}", correlationId, resultId);

            Response.Headers[Metadata.CorrelationIdKey] = correlationId;
            return new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };
        }

        //private async Task<NetworkIdentity> CreateActionItem(Entities.ActionItem item, string userId, string correlationId)
        //{
        //    item.Id ??= NetworkIdentity.New().Value.ToString();
        //    return await _executionClient.CreateActionItem(
        //        new[] { item.AsAggregate() },
        //        NetworkIdentity.From(userId),
        //        correlationId);
        //}

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: ExecutionController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ExecutionController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ExecutionController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ExecutionController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ExecutionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ExecutionController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ExecutionController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
