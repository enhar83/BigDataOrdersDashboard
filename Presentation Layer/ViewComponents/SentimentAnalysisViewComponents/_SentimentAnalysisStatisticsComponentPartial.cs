using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.SentimentAnalysisViewComponents
{
    public class _SentimentAnalysisStatisticsComponentPartial:ViewComponent
    {
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        public _SentimentAnalysisStatisticsComponentPartial(ISentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var customerStatistics = _sentimentAnalysisService.GetCustomerStatistics(id);
            return View(customerStatistics);
        }
    }
}
