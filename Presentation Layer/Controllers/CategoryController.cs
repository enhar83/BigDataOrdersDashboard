using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult CategoryList()
        {
            return View();
        }
    }
}
