using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Models;

namespace Presentation_Layer.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public StatisticsController(ICategoryService categoryService, IProductService productService, ICustomerService customerService, IOrderService orderService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var model = new StatisticsIndexCartsViewModel
            {
                CategoryCount = _categoryService.CountCategories(),
                ProductCount = _productService.CountProducts(),
                CustomerCount = _customerService.CountCustomers(),
                OrderCount = _orderService.CountOrders(),
                CompletedOrderCount = _orderService.CountCompletedOrders(),
                CancelledOrderCount = _orderService.CountCancelledOrders(),
                DistinctCityCount = _customerService.GetCityNumber(),
                DistinctCountryCount = _customerService.GetCountryNumber(),
                MostOrderingCountry = _orderService.GetMostOrderingCountry(),
                MostOrderingCustomer = _orderService.GetMostOrderingCustomer(),
                ThisYearOrders = _orderService.GetThisYearOrders(),
                MostProducedProductCountry = _productService.GetCountryMostProducesProduct()
            };

            return View(model);
        }
    }
}
