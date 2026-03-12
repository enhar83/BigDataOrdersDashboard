using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CustomerLoyaltyController : Controller
    {
        private readonly ICustomerLoyaltyService _customerLoyaltyService;

        public CustomerLoyaltyController(ICustomerLoyaltyService customerLoyaltyService)
        {
            _customerLoyaltyService = customerLoyaltyService;
        }

        public IActionResult ItalyLoyaltyScores()
        {
            var customerLoyalytScores = _customerLoyaltyService.GetCustomerLoyaltyScores();
            return View(customerLoyalytScores);
        }
    }
}
