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

namespace Business_Layer.MachineLearning.Concrete
{
    public class CustomerLoyaltyManager : ICustomerLoyaltyService
    {
        private readonly IUnitOfWork _uow;
        private readonly MLContext _mlContext;
        private readonly string _modelPath = "wwwroot/mlmodels/LoyaltyScoreModel.zip";

        public CustomerLoyaltyManager(IUnitOfWork uow)
        {
            _uow = uow;
            _mlContext = new MLContext(seed: 0);
        }

        public List<CustomerLoyaltyScoreDto> GetCustomerLoyaltyScores(string cityName)
        {
            var date = new DateTime(2025, 12, 31);

            var rawData = _uow.Customers.GetQueryable()
                .AsNoTracking()
                .Where(c => c.CustomerCity == cityName)
                .Select(g => new
                {
                    CustomerName = g.CustomerName + " " + g.CustomerSurname,
                    OrderCount = g.Orders.Count(),
                    TotalSpend = g.Orders.Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice),
                    LastOrderDate = g.Orders.Any() ? g.Orders.Max(o => (DateTime?)o.OrderDate) : null
                })
                .ToList();

            return rawData.Select(x =>
            {
                var daySinceLastOrder = (x.LastOrderDate.HasValue)
                    ? (date - x.LastOrderDate.Value).TotalDays
                    : 999;

                double recenyScore = daySinceLastOrder switch
                {
                    <= 1 => 100, 
                    <= 3 => 80,
                    <= 7 => 60,
                    <= 15 => 40,
                    <= 25 => 20,
                    _ => 0 
                };

                double frequencyScore = x.OrderCount switch
                {
                    >= 600 => 100,
                    >= 400 => 80,
                    >= 200 => 60,
                    >= 100 => 40,
                    >= 50 => 20,
                    _ => 5
                };

                double monetaryScore = x.TotalSpend switch
                {
                    >= 500000 => 100,
                    >= 300000 => 80,
                    >= 100000 => 60,
                    >= 50000 => 40,
                    >= 10000 => 20,
                    _ => 5
                };

                double loyaltyScore = (recenyScore * 0.4) + (frequencyScore * 0.3) + (monetaryScore * 0.3);

                return new CustomerLoyaltyScoreDto
                {
                    CustomerName = x.CustomerName,
                    OrderCount = x.OrderCount,
                    LastOrderDate = x.LastOrderDate,
                    TotalSpend = Math.Round(x.TotalSpend, 2),
                    LoyaltyScore = Math.Round(loyaltyScore, 2)
                };
            })
            .OrderByDescending(x => x.LoyaltyScore)
            .ToList();
        }

        public List<CustomerLoyaltyScoreResultMLDto> GetCustomerLoyaltyScoresWithML(string cityName)
        {
            var date = new DateTime(2025, 12, 31);
            ITransformer model;

            var rawData = _uow.Customers.GetQueryable()
                .AsNoTracking() //veri üzerinde değişiklik yapılmayacağından dolayı takip mekanizması kapatıldı. readonly bir işlem
                .Where(c => c.CustomerCity == cityName)
                .Select(g => new
                {
                    CustomerName = g.CustomerName + " " + g.CustomerSurname,
                    OrderCount = g.Orders.Count(), //frequency
                    TotalSpend = g.Orders.Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice), //monetary
                    LastOrderDate = g.Orders.Any() ? g.Orders.Max(o => (DateTime?)o.OrderDate) : null //recency
                })
                .ToList();

            //db objesi, ml.net'in tanıyacağı CustomerLoyaltyScoreMLDto nesnesine çevrilir
            var trainingData = rawData.Select(x => new CustomerLoyaltyScoreDataMLDto
            {
                CustomerName = x.CustomerName,
                Frequency = (float)x.OrderCount, //ml.net float ile çalıştığından dolayı dönüşüm yapılır
                Monetary = (float)x.TotalSpend,
                Recency = x.LastOrderDate.HasValue ? (float)(date - x.LastOrderDate.Value).TotalDays : 999f, //tarih farkı gün cinsinden sayıya dökülüyor
                LoyaltyScore = CalculateManualScore(x.OrderCount, x.TotalSpend, x.LastOrderDate, date)
            }).ToList();

            //eğer eğitilmiş model dosyası diskte varsa, onu yükleyip kullanıyoruz
            if (File.Exists(_modelPath))
            {
                DataViewSchema modelSchema;
                model = _mlContext.Model.Load(_modelPath, out modelSchema);
            }
            else
            {
                //c# listesi (traningData), ml.net kütüphanesinin üzerinde matematiksel işlem yapabileceği özel bir tablı formatına yüklenir
                IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                //concatenate: ai kolonlara tek tek bakmaz. r,f,m değerlerini birleştirip features adında tek bir paket yapar.
                //sdca: bir sayı tahmin etme (regression) problemidir denir ve SDCA algoritması eklenir. labelColumnName: "LoyaltyScore" diyerek de öğrenmen gereken hedef değer budur denir.
                var pipeline = _mlContext.Transforms
                    .Concatenate("Features", "Recency", "Frequency", "Monetary")
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features")) //böylelikle features içerisinde tüm veriler 0-1 arasında ölçeklendirilir ve doğruluk artar
                    .Append(_mlContext.Regression.Trainers.Sdca(
                        labelColumnName: "LoyaltyScore", //modelin tahmin etmeye çalıştığı hedef değişken (label) belirlenir.
                        maximumNumberOfIterations: 100) //modelin kaç kez kendi iç optimizasyon döngüsünü çalıştıracağını belirtir.
                    );

                //ai eğitimi başlar. algoritma dataView içerisinde verilere bakar ve müşterilerin R-F-M değerleri arasındaki matematiksel bağı çözer
                model = pipeline.Fit(dataView);

                //modeli kaydetme işlemi
                var directory = Path.GetDirectoryName(_modelPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
                _mlContext.Model.Save(model, dataView.Schema, _modelPath);
            }

            //tekil tahminler yapmak için motor oluşturur
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CustomerLoyaltyScoreDataMLDto, CustomerLoyaltyScorePredictionMLDto>(model);

            //her bir müşterinin verisini modele sorar. senin bu müşteri için tahminin ne?
            //Sonuçları kullanıcıya gösterilecek olan Result DTO'ya aktarıyoruz
            return trainingData.Select(item => {
                var prediction = predictionEngine.Predict(item);

                return new CustomerLoyaltyScoreResultMLDto
                {
                    CustomerName = item.CustomerName,
                    Recency = (double)item.Recency,
                    Frequency = (double)item.Frequency,
                    Monetary = (double)item.Monetary,
                    ActualLoyaltyScore = Math.Round(item.LoyaltyScore, 2), // Senin kurallarınla hesaplanan puan
                    PredictedLoyaltyScore = Math.Round(prediction.LoyaltyScore, 2) // ML'den gelen tahmin skoru
                };
            })
            .OrderByDescending(x => x.PredictedLoyaltyScore)
            .ToList();
        }

        //yardımcı bir metottur.
        //girdilere göre doğru cevabın ne olduğunu öğrendiği yerdir. ML modeline bir rehberlik eder.
        //eğer bu kısım olmazsa modelin elinde bir girdi R,F,M olur ama sonuç olmaz. sonuç olmayınca neyi öğreneceğini şaşırır. bu öğretmeni olmayan ve elinde cevap anahtarı bulunmayan bir öğrencinin sınava girmesine benzer.
        //bu metot tamamen sıkı hesaplamalar yapmaz. bu metot sayesinde eğrili bir grafik oluyor gibi düşünülebilir. mesela ml olmadan yazılan metotta 1 tl 20 puan farkedebilirdi ama şu an eğrisel olduğundan dolayı daha esnek bir hesaplama yapılmaktadır.
        private float CalculateManualScore(int orderCount, double totalSpend, DateTime? lastOrderDate, DateTime baseDate)
        {
            double daySinceLastOrder = lastOrderDate.HasValue
                ? (baseDate - lastOrderDate.Value).TotalDays
                : 999;

            float recencyScore = daySinceLastOrder switch
            {
                <= 1 => 100,
                <= 2 => 90,
                <= 3 => 80,
                <= 5 => 70,
                <= 7 => 60,
                <= 10 => 50,
                <= 13 => 40,
                <= 16 => 30,
                <= 20 => 20,
                <= 25 => 10,
                _ => 0
            };

            float frequencyScore = orderCount switch
            {
                >= 390 => 100,
                >= 380 => 90,
                >= 370 => 80,
                >= 360 => 70,
                >= 350 => 60,
                >= 340 => 50,
                >= 330 => 40,
                >= 320 => 30,
                >= 310 => 20,
                >= 300 => 10,
                _ => 5
            };

            float monetaryScore = totalSpend switch
            {
                >= 800000 => 100,
                >= 765000 => 90,
                >= 730000 => 80,
                >= 695000 => 70,
                >= 660000 => 60,
                >= 625000 => 50,
                >= 590000 => 40,
                >= 555000 => 30,
                >= 520000 => 20,
                >= 495000 => 10,
                _ => 5
            };

            float finalScore = (recencyScore * 0.4f) + (frequencyScore * 0.3f) + (monetaryScore * 0.3f);

            return (float)Math.Round(finalScore, 2);
        }

        public List<string> GetDistictCityNames(string countryName)
        {
            return _uow.Customers.GetQueryable()
                .Where(c => c.CustomerCountry == countryName)
                .Select(c => c.CustomerCity)
                .Distinct().
                ToList(); 
        }

        public List<string> GetDistictCountryNames()
        {
            return _uow.Customers.GetQueryable()
                .Select(c => c.CustomerCountry)
                .Distinct()
                .ToList();
        }
    }
}
