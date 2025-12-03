using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult CustomerList(int page=1)
        {
            int pageSize = 12;
            var (customers, totalCount) = _customerService.GetCustomersWithPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(customers);
        }
    }
}
