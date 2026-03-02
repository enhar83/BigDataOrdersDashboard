using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Models;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubStatisticsComponentPartial:ViewComponent
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;

        public _DashboardSubStatisticsComponentPartial(IOrderService orderService, IProductService productService, ICategoryService categoryService, ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new DashboardMiniCartsViewModel
            {
                CategoryCount = _categoryService.CountCategories(),
                ProductCount = _productService.CountProducts(),
                CustomerCount = _customerService.CountCustomers(),
                OrderCount = _orderService.CountOrders(),
                StockAlertCount = _productService.GetStockAlertProductCount(),
                OrderCountInThisMonth = _orderService.GetOrdersCountInThisMonth()
            };

            return View(model);
        }
    }
}
