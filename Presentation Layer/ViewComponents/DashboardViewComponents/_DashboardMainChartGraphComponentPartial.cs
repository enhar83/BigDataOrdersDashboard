using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardMainChartGraphComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardMainChartGraphComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _orderService.Last6MonthsSalesGraph();
            return View(values);
        }
    }
}
