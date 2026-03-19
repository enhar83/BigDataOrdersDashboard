using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.SentimentAnalysisViewComponents
{
    public class _SentimentAnalysisAIAnalysisByReviewsComponentPartial:ViewComponent
    {
        private readonly ISentimentAnalysisService _sentimentAnalysisService;

        public _SentimentAnalysisAIAnalysisByReviewsComponentPartial(ISentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
