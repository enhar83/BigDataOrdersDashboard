using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Presentation_Layer.Models.MachineLearningViewModels;

namespace Presentation_Layer.Controllers
{
    public class ForecastController : Controller
    {
        private readonly IPredictionService _predictionService;

        public ForecastController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public IActionResult PaymentMethodForecast(string paymentMethod)
        {
            //tahmin motorunu her bir yöntem için ayrı ayrı çalıştırmak gerekiyor.
            var paymentMethods = _predictionService.GetDistinctPaymentMethods();

            if (string.IsNullOrEmpty(paymentMethod) && paymentMethods.Any())
                paymentMethod = paymentMethods.First();

            var viewModel = new PaymentForecastViewModel
            {
                PaymentMethods = paymentMethods,
                SelectedPaymentMethod = paymentMethod,
                ForecastResults = new List<PaymentForecastResultDto>()
            };

            if (!string.IsNullOrEmpty(paymentMethod))
            {
                //sıradaki metot için managerdaki metodu çalıştırır, tahmin ve 2025 verisi elde edilir.
                var (prediction, actuals) = _predictionService.GetPaymentMethodForecast(paymentMethod);

                //managerdan gelen 3 aylık paketi parçalara ayırır ve tek tek satır haline getirir.
                for (int i = 0; i < 3; i++)
                {
                    var lastYearValue = actuals.FirstOrDefault(x => x.MonthIndex == (i + 1))?.OrderCount ?? 0;

                    viewModel.ForecastResults.Add(new PaymentForecastResultDto
                    {
                        PaymentMethod = paymentMethod,
                        Month = new DateTime(2026, i + 1, 1).ToString("MMMM yyyy"),
                        PredictedCount = (int)Math.Max(0, prediction.ForecastedValues[i]), //floatı integera çevirir ve eksi bir sonuç gelme ihtimalin karşı onu 0 olarak gösterir.
                        LastYearsCount = (int)lastYearValue
                    });
                }

            }

            return View(viewModel);
        }

        public IActionResult CitiesForecast(string countryName, string cityName)
        {
            var countries = _predictionService.GetDistinctCountries();
            if (string.IsNullOrEmpty(countryName)) countryName = "Türkiye";

            var cities = _predictionService.GetDistinctCityNames(countryName);
            if (string.IsNullOrEmpty(cityName) || !cities.Contains(cityName))
                cityName = cities.FirstOrDefault();


            var viewModel = new CitiesForecastViewModel
            {
                CountryNames=countries,
                SelectedCountry=countryName,
                CityNames = cities,
                SelectedCity = cityName,
                ForecastResults = new List<CitiesForecastResultDto>()
            };

            if (!string.IsNullOrEmpty(cityName))
            {
                var (prediction, actuals) = _predictionService.GetCitiesForecast(cityName);

                if (prediction.ForecastedValues.Any(v => v > 0))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var dataFor2024 = actuals.FirstOrDefault(x => x.MonthIndex == (i + 1))?.OrderCount ?? 0;
                        var dataFor2025 = actuals.FirstOrDefault(x => x.MonthIndex == (i + 13))?.OrderCount ?? 0;

                        int predicted = (int)Math.Max(0, prediction.ForecastedValues[i]);


                        double changeRate = 0;
                        if (dataFor2025 > 0)
                        {
                            changeRate = (double)(predicted - dataFor2025) / dataFor2025 * 100;
                        }

                        viewModel.ForecastResults.Add(new CitiesForecastResultDto
                        {
                            CountryName=countryName,
                            CityName = cityName,
                            Month = new DateTime(2026, i + 1, 1).ToString("MMMM yyyy"),
                            PredictedCount = predicted,
                            CountFor2024 = (int)dataFor2024,
                            CountFor2025 = (int)dataFor2025,
                            ChangeRate = changeRate
                        });
                    }
                }
            }

            return View(viewModel);
        }
    }
}
