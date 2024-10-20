using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : Controller
    {
        // GET: ExecutionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExecutionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExecutionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExecutionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExecutionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExecutionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExecutionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExecutionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
