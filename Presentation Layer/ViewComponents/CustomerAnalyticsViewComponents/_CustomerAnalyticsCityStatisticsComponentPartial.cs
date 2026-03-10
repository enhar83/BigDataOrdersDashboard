using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsCityStatisticsComponentPartial:ViewComponent
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public _CustomerAnalyticsCityStatisticsComponentPartial(ICustomerAnalyticsService customerAnalyticsService)
        {
            _customerAnalyticsService = customerAnalyticsService;
        }

        public IViewComponentResult Invoke()
        {
            var citiesInformation = _customerAnalyticsService.GetCityOrderCountForDonut();
            return View(citiesInformation);
        }
    }
}
