using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CustomerAnalytics : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
