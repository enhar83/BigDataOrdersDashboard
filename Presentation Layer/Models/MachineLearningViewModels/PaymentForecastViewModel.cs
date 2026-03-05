using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Presentation_Layer.Models.MachineLearningViewModels
{
    public class PaymentForecastViewModel
    {
        public string SelectedPaymentMethod { get; set; }
        public List<string> PaymentMethods { get; set; }
        public List<PaymentForecastResultDto> ForecastResults { get; set; }
    }
}

/*
    * PaymentForecastResultDto: Sadece tek bir satırın bilgisini tutar. { PaymentMethod: "PayPal", Month: "Ocak 2026", PredictedCount: 133 }
    * PaymentForecastViewModel: View tarafının ihtiyaç duyduğu tüm farklı veri tiplerini tek bir pakette toplamaktadır.
 */
