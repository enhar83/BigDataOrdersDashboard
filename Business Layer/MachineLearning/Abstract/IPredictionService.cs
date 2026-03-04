using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Business_Layer.MachineLearning.Abstract
{
    public interface IPredictionService
    {
        (PaymentForecastPredictionDTO prediction, List<PaymentForecastDataDTO> actuals) GetPaymentMethodForecast(string paymentMethod); //eğitilmiş modelden tahmin almak için
        List<string> GetDistinctPaymentMethods();
    }
}
