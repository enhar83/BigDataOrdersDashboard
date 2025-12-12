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

        public IActionResult NumericStatistics()
        {
            var model = new NumericStatisticsCartsViewModel
            {
                CategoryCount = _categoryService.CountCategories(),
                ProductCount = _productService.CountProducts(),
                CustomerCount = _customerService.CountCustomers(),
                OrderCount = _orderService.CountOrders(),
                CompletedOrderCount = _orderService.CountCompletedOrders(),
                CancelledOrderCount = _orderService.CountCancelledOrders(),
                DistinctCityCount = _customerService.GetCityNumber(),
                DistinctCountryCount = _customerService.GetCountryNumber(),
                ThisYearOrders = _orderService.GetThisYearOrders(),
                TotalProductStock = _productService.GetTotalProductStock(),
                TotalRevenue = _orderService.GetTotalRevenue(),
                AverageTotalRevenue = _orderService.GetAverageRevenue()
            };

            return View(model);
        }

        public IActionResult TextualStatistics()
        {
            var model = new TextualStatisticsCartsViewModel
            {
                MostExpensiveProduct = _productService.GetMostExpensiveProductName(),
                CustomerWithMostOrders = _orderService.GetMostOrderedCustomer(),
                CategoryWithMostOrders = _orderService.GetMostOrderedCategory(),
                CityWithMostOrders = _orderService.GetMostOrderedCity(),
                CountryWithMostOrders = _orderService.GetMostOrderedCountry(),
                LeastStockedProduct = _productService.GetLeastStockedProductName(),
                MostOrderedProductThisMonth = _orderService.GetMostOrderedProductThisMonth(),
                MostCancelledProduct = _orderService.GetMostCancelledProduct(),
                LastAddedCustomer = _customerService.GetLastAddedCustomer(),
                FirstAddedCustomer = _customerService.GetFirstAddedCustomer(),
                MostOrderedPayment = _orderService.GetMostOrderedPayment()
            };

            return View(model);
        }
    }
}
