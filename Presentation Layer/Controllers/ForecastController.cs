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

        public IActionResult PaymentMethodForecast()
        {
            //tahmin motorunu her bir yöntem için ayrı ayrı çalıştırmak gerekiyor.
            var paymentMethods = _predictionService.GetDistinctPaymentMethods(); 

            var viewModel = new PaymentForecastViewModel
            {
                ForecastResults = new List<PaymentForecastResultDto>()
            };

            foreach (var method in paymentMethods)
            {
                //sıradaki metot için managerdaki metodu çalıştırır, tahmin ve 2025 verisi elde edilir.
                var (prediction,actuals) = _predictionService.GetPaymentMethodForecast(method);

                //managerdan gelen 3 aylık paketi parçalara ayırır ve tek tek satır haline getirir.
                for (int i = 0; i < 3; i++)
                {
                    var lastYearValue = actuals.FirstOrDefault(x => x.MonthIndex == (i + 1))?.OrderCount ?? 0;

                    viewModel.ForecastResults.Add(new PaymentForecastResultDto
                    {
                        PaymentMethod = method,
                        Month = new DateTime(2026, i + 1, 1).ToString("MMMM yyyy"),
                        PredictedCount = (int)Math.Max(0, prediction.ForecastedValues[i]), //floatı integera çevirir ve eksi bir sonuç gelme ihtimalin karşı onu 0 olarak gösterir.
                        LastYearsCount = (int)lastYearValue
                    });
                }
            }

            return View(viewModel);
        }

        public IActionResult GermanCitiesForecast()
        {
            var germanCities = _predictionService.GetDistinctCityNames();

            var viewModel = new GermanyCitiesForecastViewModel
            {
                ForecastResults = new List<GermanyCitiesForecastResultDto>()
            };

            foreach(var cityName in germanCities)
            {
                var (prediction, actuals) = _predictionService.GetGermanyCitiesForecast(cityName);

                for (int i=0;i<6;i++) //i değeri horizon değerine bağlı olarak belirleniyor. 6 horizon değeri olduğu için i<6
                {
                    //2025 verilerini gösterecek, 2024'ü göstermek istesek i+1 yapılırdı.
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

            return View(viewModel);
        }
    }
}
