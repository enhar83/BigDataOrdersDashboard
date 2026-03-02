using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubStatisticsComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardSubStatisticsComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
