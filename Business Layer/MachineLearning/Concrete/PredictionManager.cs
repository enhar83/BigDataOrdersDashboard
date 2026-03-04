using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace Business_Layer.MachineLearning.Concrete
{
    public class PredictionManager : IPredictionService
    {
        private readonly IUnitOfWork _uow;
        private readonly MLContext _mlContext; //ML.NET'in ana motoru, beynidir. Tüm yapay zeka işlemleri bu nesne üzerinden yürütülür.
        

        public PredictionManager(IUnitOfWork uow)
        {
            _uow = uow;
            _mlContext = new MLContext(seed: 0); //her zaman aynı sonuçları almak için seed sabitlendi
        }

        #region PaymentMethodForecast

        public PaymentForecastPredictionDTO GetPaymentMethodForecast(string paymentMethod)
        {
            var rawData = _uow.Orders.GetQueryable()
                .Where(o => o.OrderDate.Year == 2025 && o.PaymentMethod == paymentMethod)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month }) //verileri aylık paketler haline getirir
                .Select(g => new
                {
                    OrderCount = (float)g.Count() //SQLden geşem her bir aylık paket için sipariş sayısını bulur ve ML.NET için floata çevirir. Kısacası veriyi makineye uygun hale getirir.
                })
                .ToList(); //veriler rame geldi

            var methodData = rawData.Select((x, index) => new PaymentForecastDataDTO //SQLden gelen ham sayı listesine zamanın akışını temsil eden bir index vermek için kullanılır.
            {
                PaymentMethod = paymentMethod,
                MonthIndex = index + 1,
                OrderCount = x.OrderCount
            }).ToList(); //[150,180,210] geliyorsa 1.Ay:150, 2.Ay:180, 3.Ay:210 olarak değiştirir. 

            //Bitcoin adında bir ödeme yöntemi olduğunu varsay ve hiç sipariş edilmemiş bu yöntemle. ML.NET boş veriyle çalışamayacağından dolayı eğitilemez. Bir nevi sigorta görevi görür.
            if (!methodData.Any()) return new PaymentForecastPredictionDTO { ForecastedValues = new float[] { 0, 0, 0 } };

            var dataView = _mlContext.Data.LoadFromEnumerable(methodData); //hazırlanan C# listesini ML.NET'in anlayacağı özel bir tablo formatına dönüştürür. (IDataView)
            //SQLden gelen veriler karışık bir deftere benzediği için LoadFromEnumerable diyerek bu notları, yapay zekanın çok hızlı okuyabildiği dijital bir Excel tablosuna dönüştürür.

            //Verinin ham halden, işlenen bir modele dönüşene kadar izlediği yoldur.
            //SSA geçmişteki iniş çıkışlara bakar, gürültülü verileri eler ve gerçek trendi bulur.
            var pipeline = _mlContext.Forecasting.ForecastBySsa( //SSA algoritmasını yapılandırır. (Singular Spectrum Analysis, zaman içindeki verileri inceleyen özel bir matematiksel modeldir)
                outputColumnName: nameof(PaymentForecastPredictionDTO.ForecastedValues),
                inputColumnName: nameof(PaymentForecastDataDTO.OrderCount),
                windowSize: 4, //4 aylık dönemlerle geçmişe bakar.
                seriesLength: methodData.Count, 
                trainSize: methodData.Count,
                horizon: 3, //gelecek 3 ayı hedef alır.
                confidenceLevel: 0.95f);

            //hazırlanan pipeline'ın verilerle buluşup öğrenmeye başladığı andır.
            var model = pipeline.Fit(dataView); //öğrenme işleminin başladığı yerdir. Algoritma 2025 verilerindeki matematiksel örüntüyü burada çözer.

            //eğitilmiş modelden çok hızlı bir şekilde sonuç almak için oluşturulan özel yapıdır.
            //eğitim bitmiş haldedir ve artık soru cevap yapılır. PayPal için Ocak 2026 ne olur gibi.
            var forecastEngine = model.CreateTimeSeriesEngine<PaymentForecastDataDTO, PaymentForecastPredictionDTO>(_mlContext); 

            //sonucun verildiği yerdir.
            //bu satır çalıştığında elde artık bir PaymentForecastPredictionDto vardır ve içerisinde 3 aylık tahmin bulunur.
            return forecastEngine.Predict();
        }

        public List<string> GetDistinctPaymentMethods()
        {
            return _uow.Orders.GetQueryable()
                .Select(x => x.PaymentMethod)
                .Distinct()
                .ToList();
        }
        #endregion
    }
}
