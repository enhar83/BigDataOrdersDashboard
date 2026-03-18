using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents;

namespace Presentation_Layer.ViewComponents.SentimentAnalysisViewComponents
{
    public class _SentimentAnalysisMainCoverTableComponentPartial:ViewComponent
    {
        private readonly ISentimentAnalysisService _sentimentAnalysisService;

        public _SentimentAnalysisMainCoverTableComponentPartial(ISentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var customerInformations = _sentimentAnalysisService.GetCustomerInformations(id);

            return View(customerInformations);
        }
    }
}
