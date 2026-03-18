using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class SentimentAnalysisController : Controller
    {
        public IActionResult CustomerInsights()
        {
            return View();
        }
    }
}
