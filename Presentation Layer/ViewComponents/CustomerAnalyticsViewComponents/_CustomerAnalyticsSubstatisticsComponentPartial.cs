using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsSubstatisticsComponentPartial:ViewComponent
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public _CustomerAnalyticsSubstatisticsComponentPartial(ICustomerAnalyticsService customerAnalyticsService)
        {
            _customerAnalyticsService = customerAnalyticsService;
        }

        public IViewComponentResult Invoke()
        {
            var customerStatistics = _customerAnalyticsService.GetCustomerAnalyticsSubstatistics();
            return View(customerStatistics);
        }
    }
}
