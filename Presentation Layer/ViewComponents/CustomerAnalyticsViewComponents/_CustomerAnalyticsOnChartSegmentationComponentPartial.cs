using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsOnChartSegmentationComponentPartial:ViewComponent
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public _CustomerAnalyticsOnChartSegmentationComponentPartial(ICustomerAnalyticsService customerAnalyticsSerive)
        {
            _customerAnalyticsService = customerAnalyticsSerive;
        }

        public IViewComponentResult Invoke()
        {
            var customerSegments = _customerAnalyticsService.GetCustomerSegmentForChart();
            return View(customerSegments);
        }
    }
}
