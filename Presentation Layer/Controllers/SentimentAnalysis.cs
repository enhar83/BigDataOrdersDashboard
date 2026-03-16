using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class SentimentAnalysis : Controller
    {
        private readonly ISentimentAnalysisService _sentimentAnalysisService;

        public SentimentAnalysis(ISentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
