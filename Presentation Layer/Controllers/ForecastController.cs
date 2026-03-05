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

        public IActionResult GermanCitiesForecast(string cityName)
        {
            var countryName = "Almanya";
            var germanCities = _predictionService.GetDistinctCityNames(countryName);

            if (string.IsNullOrEmpty(cityName) && germanCities.Any())
                cityName =germanCities.First();

            var viewModel = new GermanyCitiesForecastViewModel
            {
                CityNames = germanCities,
                SelectedGermanyCity = cityName,
                ForecastResults = new List<GermanyCitiesForecastResultDto>()
            };

            if (!string.IsNullOrEmpty(cityName))
            {
                var (prediction, actuals) = _predictionService.GetGermanyCitiesForecast(cityName);

                if (prediction.ForecastedValues.Any(v => v > 0))
                {
                    for (int i = 0; i < 6; i++) //horizon değerine bağlı olarak değişir.
                    {
                        var lastYearValue = actuals.FirstOrDefault(x => x.MonthIndex == (i + 13))?.OrderCount ?? 0;

                        viewModel.ForecastResults.Add(new GermanyCitiesForecastResultDto
                        {
                            CityName = cityName,
                            Month = new DateTime(2026, i + 1, 1).ToString("MMMM yyyy"),
                            PredictedCount = (int)Math.Max(0, prediction.ForecastedValues[i]),
                            LastYearsCount = (int)lastYearValue
                        });
                    }
                }
            }

            return View(viewModel);
        }

        public IActionResult TurkeyCitiesForecast(string cityName)
        {
            var countryName = "Türkiye";
            var turkeyCities = _predictionService.GetDistinctCityNames(countryName);

            if (string.IsNullOrEmpty(cityName) && turkeyCities.Any())
                cityName = turkeyCities.First();

            var viewModel = new TurkeyCitiesForecastViewModel
            {
                CityNames = turkeyCities,
                SelectedTurkeyCity = cityName,
                ForecastResults = new List<TurkeyCitiesForecastResultDto>()
            };

            if (!string.IsNullOrEmpty(cityName))
            {
                var (prediction, actuals) = _predictionService.GetTurkeyCitiesForecast(cityName);

                if (prediction.ForecastedValues.Any(v => v > 0))
                {
                    for (int i = 0; i < 6; i++) 
                    {
                        var lastYearValue = actuals.FirstOrDefault(x => x.MonthIndex == (i + 13))?.OrderCount ?? 0;

                        viewModel.ForecastResults.Add(new TurkeyCitiesForecastResultDto
                        {
                            CityName = cityName,
                            Month = new DateTime(2026, i + 1, 1).ToString("MMMM yyyy"),
                            PredictedCount = (int)Math.Max(0, prediction.ForecastedValues[i]),
                            LastYearsCount = (int)lastYearValue
                        });
                    }
                }
            }

            return View(viewModel);
        }
    }
}
