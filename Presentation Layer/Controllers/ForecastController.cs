using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class ForecastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
