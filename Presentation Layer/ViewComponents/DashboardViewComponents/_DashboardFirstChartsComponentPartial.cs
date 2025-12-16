using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardFirstChartsComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardFirstChartsComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _orderService.GetOrderStatusChartData();

            return View(values);
        }
    }
}
