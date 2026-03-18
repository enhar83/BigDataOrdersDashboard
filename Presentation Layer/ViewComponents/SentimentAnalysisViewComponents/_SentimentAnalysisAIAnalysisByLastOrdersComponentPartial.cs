using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.SentimentAnalysisViewComponents
{
    public class _SentimentAnalysisAIAnalysisByLastOrdersComponentPartial:ViewComponent
    {
        private readonly ISentimentAnalysisService _sentimentAnalysisService;

        public _SentimentAnalysisAIAnalysisByLastOrdersComponentPartial(ISentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var values = await _sentimentAnalysisService.GetCustomerComprehensiveAnalysisAsync(id);
            return View(values);
        }
    }
}
