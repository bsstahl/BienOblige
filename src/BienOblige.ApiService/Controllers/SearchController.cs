using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BienOblige.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public SearchController()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
