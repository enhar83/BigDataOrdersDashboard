using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
