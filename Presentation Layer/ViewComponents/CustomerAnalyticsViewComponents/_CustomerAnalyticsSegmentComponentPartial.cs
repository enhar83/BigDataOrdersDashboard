using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsSegmentComponentPartial:ViewComponent
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public _CustomerAnalyticsSegmentComponentPartial(ICustomerAnalyticsService customerAnalyticsService)
        {
            _customerAnalyticsService = customerAnalyticsService;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
