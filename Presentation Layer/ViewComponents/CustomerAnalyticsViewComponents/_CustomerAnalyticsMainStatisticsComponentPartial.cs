using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsMainStatisticsComponentPartial:ViewComponent
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public _CustomerAnalyticsMainStatisticsComponentPartial(ICustomerAnalyticsService customerAnalyticsService)
        {
            _customerAnalyticsService = customerAnalyticsService;
        }

        public IViewComponentResult Invoke()
        {
            var mainStatistics = _customerAnalyticsService.GetCustomerAnalyticsMainStatistics();
            return View(mainStatistics);
        }
    }
}
