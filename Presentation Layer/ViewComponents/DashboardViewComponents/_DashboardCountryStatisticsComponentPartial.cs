using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardCountryStatisticsComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardCountryStatisticsComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var countries = _orderService.GetCountryOrderCountForDonut();
            return View(countries);
        }
    }
}
