using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductList()
        {
            return View();
        }
    }
}
