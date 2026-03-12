using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CustomerLoyaltyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
