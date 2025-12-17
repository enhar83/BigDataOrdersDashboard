using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardKpiComponentPartial : ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardKpiComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var value = _orderService.CompareTodayAndYesterdayOrdersForKpiCarts();

            return View(value);
        }
    }
}
