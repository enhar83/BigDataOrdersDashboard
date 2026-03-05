using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.EntityFrameworkCore;
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

        public (PaymentForecastPredictionDTO prediction, List<PaymentForecastDataDTO> actuals) GetPaymentMethodForecast(string paymentMethod)
        {
            //sadece 2025 yılı için çalışıldığı için OrderBy kullanmaya gerek yok zaten SQL verileri sıralı veiryor.
            //ancak 2 yıl ve üzeri için çalışılsaydı 2025 verileri 2024 verilerinin arasına karılabilir. Ocak 2025, Ocak 2024, Şubat 2025 şeklinde olabilirdi. Böyle olursa da algoritma trendi tamamen yanlış anlar.
            var rawData = _uow.Orders.GetQueryable()
                .Where(o => o.OrderDate.Year == 2025 && o.PaymentMethod == paymentMethod)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month }) //verileri aylık paketler haline getirir. Ayrıca ekstra olarak payment methodlara göre gruplamaya gerek yok çünkü Where koşulunda zaten SQL o şehre ait olan verileri getiriyor.
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
            if (!methodData.Any()) return (new PaymentForecastPredictionDTO { ForecastedValues = new float[] { 0, 0, 0 } }, methodData);

            var dataView = _mlContext.Data.LoadFromEnumerable(methodData); //hazırlanan C# listesini ML.NET'in anlayacağı özel bir tablo formatına dönüştürür. (IDataView)
            //SQLden gelen veriler karışık bir deftere benzediği için LoadFromEnumerable diyerek bu notları, yapay zekanın çok hızlı okuyabildiği dijital bir Excel tablosuna dönüştürür.

            //Verinin ham halden, işlenen bir modele dönüşene kadar izlediği yoldur.
            //SSA geçmişteki iniş çıkışlara bakar, gürültülü verileri eler ve gerçek trendi bulur.
            var pipeline = _mlContext.Forecasting.ForecastBySsa( //SSA algoritmasını yapılandırır. (Singular Spectrum Analysis, zaman içindeki verileri inceleyen özel bir matematiksel modeldir)
                outputColumnName: nameof(PaymentForecastPredictionDTO.ForecastedValues), //tahminin yazılacağı sütun
                inputColumnName: nameof(PaymentForecastDataDTO.OrderCount), //tahmin yapılacak değer, OrderCount
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
            return (forecastEngine.Predict(), methodData);
        }

        public List<string> GetDistinctPaymentMethods()
        {
            return _uow.Orders.GetQueryable()
                .Select(x => x.PaymentMethod)
                .Distinct()
                .ToList();
        }

        #endregion

        #region CitiesForecast

        public (CitiesForecastPredictionDto prediction, List<CitiesForecastDataDto> actuals) GetCitiesForecast(string cityName)
        {
            var rawData = _uow.Orders.GetQueryable()
                .Include(o => o.Customer)
                .Where(o => (o.OrderDate.Year == 2024 || o.OrderDate.Year == 2025) && o.Customer.CustomerCity == cityName)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month }) //amaç şehirleri birbirinden ayırma değil, zaten tek bir şehir üzerinde çalışılıyor. Amaç siparişleri aylar halinde gruplamak.
                .OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month) //iki yıllık verinin birbirine karışmaması için kullanıldı.
                .Select(g => new
                {
                    OrderCount = (float)g.Count()
                })
                .ToList();

            var methodData = rawData.Select((x, index) => new CitiesForecastDataDto
            {
                CityName = cityName,
                MonthIndex = index + 1,
                OrderCount = x.OrderCount
            }).ToList();

            //en az 21 aylık veri ister
            if (methodData.Count < 21) //windowSize * 2 + 1
                return (new CitiesForecastPredictionDto { ForecastedValues = new float[6] }, methodData);

            //en az 50 sipariş sayısı ister yoksa veriler çok gürültülü olur ve tahminler saçma çıkabilir. Bu da algoritmanın kendini kandırmasına neden olur. 
            if (methodData.Average(x => x.OrderCount) < 50)
                return (new CitiesForecastPredictionDto { ForecastedValues = new float[6] }, methodData);

            if (!methodData.Any()) return (new CitiesForecastPredictionDto { ForecastedValues = new float[] { 0, 0, 0 } }, methodData);

            var dataView = _mlContext.Data.LoadFromEnumerable(methodData);

            var pipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(CitiesForecastPredictionDto.ForecastedValues),
                inputColumnName: nameof(CitiesForecastDataDto.OrderCount),
                windowSize: 10, //eğitim verisi (24 şu an) windowsize'ın en az 2x+1 şeklinde olmalıdır.
                seriesLength: methodData.Count,
                trainSize: methodData.Count,
                horizon: 6,
                confidenceLevel: 0.95f);

            var model = pipeline.Fit(dataView);

            var forecastEngine = model.CreateTimeSeriesEngine<CitiesForecastDataDto, CitiesForecastPredictionDto>(_mlContext);

            return (forecastEngine.Predict(), methodData);
        }

        public List<string> GetDistinctCityNames(string countryName)
        {
            return _uow.Customers.GetQueryable()
                .Where(c => c.CustomerCountry == countryName)
                .Select(c => c.CustomerCity)
                .Distinct().
                ToList();
        }

        public List<string> GetDistinctCountries()
        {
            return _uow.Customers.GetQueryable()
                .Select(c => c.CustomerCountry)
                .Distinct()
                .ToList();
        }

        

        #endregion
    }
}
