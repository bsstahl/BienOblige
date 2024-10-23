using BienOblige.Execution.Application;
using BienOblige.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : Controller
    {
        private readonly Client _executionClient;
        private readonly ILogger _logger;

        public ExecutionController(ILogger<ExecutionController> logger, Client demandClient)
        {
            _logger = logger;
            _executionClient = demandClient;
        }

        // POST api/<ExecutionController>
        [HttpPost()]
        public async Task<string> Create([FromBody] Entities.ActionItem item,
            [FromHeader(Name = "X-User-ID")] string userId,
            [FromHeader(Name = "X-Correlation-ID")] string correlationId)
        {
            // TODO: Figure out how to manage the correlation Id ideally so that it doesn't have
            // to get passed-in to the model layers since it is not needed for the
            // Application proper (just the API)

            _logger.LogInformation("Creating ActionItem with correlation ID {CorrelationId}", correlationId);

            var resultId = await _executionClient.CreateActionItem(item.AsAggregate(), NetworkIdentity.From(userId), correlationId);
            var result = resultId.Value.ToString();

            _logger.LogInformation("Created ActionItem with correlation ID {CorrelationId}. Result: {Result}", correlationId, result);

            return result;
        }

        // PATCH api/<ExecutionController>/5
        [HttpPatch("{id}")]
        public async Task Cancel(string id,
            [FromHeader(Name = "X-User-ID")] string userId,
            [FromHeader(Name = "X-Correlation-ID")] string correlationId)
            => await _executionClient.CancelActionItem(id, userId, correlationId);

        // TODO: Determine if Edit methods are necessary
        // it will depend on what types of use-cases require modifying
        // the underlying data that encompasses demand. There will be
        // separate methods in the Execution space that will be responsible
        // for making edits that represent activity on the ActionItem.
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
