using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardMapComponentPartial : ViewComponent
    {
        private readonly IOrderService _orderService;

        public _DashboardMapComponentPartial(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _orderService.GetCountryReportForMap();

            return View(values);
        }
    }
}
