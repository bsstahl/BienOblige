using BienOblige.ApiService.Entities;
using BienOblige.Execution.Application;
using BienOblige.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private const string correlationIdKey = "X-Correlation-ID";
        private const string userIdKey = "X-User-ID";

        private readonly Client _executionClient;
        private readonly ILogger _logger;

        public ExecutionController(ILogger<ExecutionController> logger, Client demandClient)
        {
            _logger = logger;
            _executionClient = demandClient;
        }

        // POST api/<ExecutionController>
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Entities.ActionItem item,
            [FromHeader(Name = userIdKey)] string userId,
            [FromHeader(Name = correlationIdKey)] string correlationId)
        {
            _logger.LogInformation("Creating ActionItem for request with correlation ID {CorrelationId}", correlationId);
            item.Id ??= NetworkIdentity.New().Value.ToString();
            var resultId = await _executionClient.CreateActionItem(
                item.AsAggregate(),
                NetworkIdentity.From(userId),
                correlationId);
            _logger.LogInformation("Created ActionItem for request with correlation ID {CorrelationId}. Result: {Result}", correlationId, resultId);

            Response.Headers.Append(correlationIdKey, correlationId);
            return new JsonResult(new CreateResponse(resultId.Value.ToString()))
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };
        }

        // POST api/<ExecutionController>/5
        [HttpPatch()]
        public async Task<IActionResult> Update(
            [FromBody] Entities.ActionItem item,
            [FromHeader(Name = userIdKey)] string userId,
            [FromHeader(Name = correlationIdKey)] string correlationId)
        {
            _logger.LogInformation("Updating ActionItem for with CorrelationId {CorrelationId}", correlationId);
            var resultId = await _executionClient.UpdateActionItem(
                item.AsAggregate(),
                NetworkIdentity.From(userId),
                correlationId);
            _logger.LogInformation("Updating ActionItem for request with CorrelationId {CorrelationId}. Result: {Result}", correlationId, resultId);

            Response.Headers.Append(correlationIdKey, correlationId);
            return new JsonResult(new CreateResponse(resultId.Value.ToString()))
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };

        }

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
