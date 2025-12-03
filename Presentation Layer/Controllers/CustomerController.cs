using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace Presentation_Layer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult CustomerList(int page = 1)
        {
            int pageSize = 12;
            var (customers, totalCount) = _customerService.GetCustomersWithPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(customers);
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomer(Customer customer)
        {
            _customerService.Add(customer);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var customerToUpdate =_customerService.GetFirstOrDefault(id);
            if (customerToUpdate == null) return NotFound();
            return View(customerToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _customerService.Update(customer);
            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomer(int id)
        {
            _customerService.Delete(id);
            return RedirectToAction("CustomerList");    
        }
    }
}
