using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
