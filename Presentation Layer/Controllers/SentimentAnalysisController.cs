using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace Presentation_Layer.Controllers
{
    public class SentimentAnalysisController : Controller
    {
        public IActionResult CustomerInsights(int id)
        {
            if (id <= 0)
                return RedirectToAction("Customer/CustomerList");

            ViewBag.SelectedId = id;
            return View();
        }

        public IActionResult ReviewInsights(int id)
        {
            if (id <= 0) 
                return RedirectToAction("Customer/CustomerList");

            ViewBag.SelectedId = id;
            return View();
        }
    }
}
