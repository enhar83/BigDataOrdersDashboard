using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardTodayOrdersComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardTodayOrdersComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var last10OrdersToday = _orderService.GetLast10OrdersToday();

            return View(last10OrdersToday);
        }
    }
}
