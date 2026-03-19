using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class SentimentAnalysisController : Controller
    {
        public IActionResult CustomerInsights(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.SelectedId = id;
            return View();
        }
    }
}
