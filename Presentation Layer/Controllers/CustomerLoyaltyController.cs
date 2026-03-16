using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Models.MachineLearningViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace Presentation_Layer.Controllers
{
    public class CustomerLoyaltyController : Controller
    {
        private readonly ICustomerLoyaltyService _customerLoyaltyService;

        public CustomerLoyaltyController(ICustomerLoyaltyService customerLoyaltyService)
        {
            _customerLoyaltyService = customerLoyaltyService;
        }

        public IActionResult LoyaltyScores(string countryName, string cityName)
        {
            var countries = _customerLoyaltyService.GetDistictCountryNames();
            if (string.IsNullOrEmpty(countryName)) countryName = "Türkiye";

            var cities = _customerLoyaltyService.GetDistictCityNames(countryName);
            if (string.IsNullOrEmpty(cityName) || !cities.Contains(cityName))
                cityName = cities.FirstOrDefault();

            var customerLoyalytScores = _customerLoyaltyService.GetCustomerLoyaltyScores(cityName);

            var vm = new CustomerLoyaltyViewModel
            {
                LoyaltyScores = customerLoyalytScores,
                Countries = countries,
                Cities = cities,
                SelectedCountry = countryName,
                SelectedCity = cityName
            };

            return View(vm);
        }

        public IActionResult LoyaltyScoresWithMl(string countryName, string cityName)
        {
            var countries = _customerLoyaltyService.GetDistictCountryNames();
            if (string.IsNullOrEmpty(countryName)) countryName = "Türkiye";

            var cities = _customerLoyaltyService.GetDistictCityNames(countryName);
            if (string.IsNullOrEmpty(cityName) || !cities.Contains(cityName))
                cityName = cities.FirstOrDefault();

            var customerLoyalytScores = _customerLoyaltyService.GetCustomerLoyaltyScoresWithML(cityName);

            var vm = new CustomerLoyaltyWithMlViewModel
            {
                LoyaltyScores = customerLoyalytScores,
                Countries = countries,
                Cities = cities,
                SelectedCountry = countryName,
                SelectedCity = cityName
            };

            return View(vm);
        }
    }
}
