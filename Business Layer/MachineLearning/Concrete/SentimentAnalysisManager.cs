using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.Configuration;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Business_Layer.MachineLearning.Concrete
{
    public class SentimentAnalysisManager : ISentimentAnalysisService
    {
        private readonly IUnitOfWork _uow;

        //pooling: arka planda bağlantıları yönetir, ihtiyaç bittiğinde onları öldürmek yerine yeniden kullanmak üzere saklar.
        //apı anahtarlarını -, base url gibi ayarları program.cs içerisinde tek bir yerden yönetmeyi saülar.
        //ancak program.cs içerisinde builder.Services.AddHttpClient(); yazılması unutulmamalıdır.
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeminiSettings _geminiSettings;
        public SentimentAnalysisManager(IUnitOfWork uow, IHttpClientFactory httpClientFactory, IOptions<GeminiSettings> geminiSettings)
        {
            _uow = uow;
            _httpClientFactory = httpClientFactory;
            _geminiSettings = geminiSettings.Value;
        }

        //async olmasının sebebi, kullanıcı istek attığı sırada bir yavaşlık olursa o sırada başka işlemlere de devam edebilmesi içindir.
        public async Task<SentimentAnalysisCustomerInsightsWithGeminiDto> GetCustomerComprehensiveAnalysisAsync(int customerId)
        {
            var query = _uow.Customers.GetQueryable()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new SentimentAnalysisCustomerInsightsWithGeminiDto
                {
                    CustomerFullName = c.CustomerName + " " + c.CustomerSurname,
                    LastOrders = c.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(20)
                    .Select(o => new SentimentAnalysisOrderDetailsForCustomerInsightsWithGeminiDto
                    {
                        OrderDate = o.OrderDate,
                        ProductName = o.Product.ProductName,
                        CategoryName = o.Product.Category.CategoryName,
                        Quantity = o.Quantity,
                        UnitPrice = (decimal)o.Product.UnitPrice,
                        TotalPrice = (decimal)(o.Quantity * o.Product.UnitPrice)
                    }).ToList()
                });

            var customerData = await query.FirstOrDefaultAsync();

            if (customerData == null) return new SentimentAnalysisCustomerInsightsWithGeminiDto();

            //ai'a gönderilecek veri hazırlanır. 
            var jsonData = JsonSerializer.Serialize(customerData);

            //ai yönlendiriliyor, prompt engineering
            //verilen promptta çıktıyı sadece html formatında üret diyerek güzel görünüm elde edilmektedir.
            string prompt = $@"
                Sen bir veri analisti ve müşteri davranış uzmanısın.
                
                Aşağıda bir müşterinin son 20 siparişine ait JSON verisi bulunmaktadır.
                Bu veriyi analiz ederek detaylı bir müşteri analiz raporu oluşturmanı istiyorum.
                
                Rapor şu alt başlıklardan oluşmalıdır:
                1. Müşteri Profili
                2. Ürün Tercihleri
                3. Zaman Bazlı Alışveriş Davranışı
                4. Ortalama Harcama ve Sıklık
                5. Sadakat ve Tekrar Harcama Eğilimi
                6. Pazarlama Önerileri
                
                ÖNEMLİ FORMAT KURALLARI:
                - Çıktıyı SADECE HTML formatında üret.
                - Alt başlıklar için <h4> kullan.
                - Paragraflar için <p> kullan.
                - Listeler için <ul> ve <li> kullan.
                - Önemli sayısal değerleri <strong> ile vurgula.
                - Gereksiz açıklama yazma, sadece HTML üret.
                
                Veri:
                {jsonData}
            ";
            var apiKey = _geminiSettings.ApiKey;
            var httpClient = _httpClientFactory.CreateClient();

            var model = "gemini-2.5-flash";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            //gemini camelcase istediğinden dolayı Contents değil contents olması önemlidir
            var requestObject = new
            {
                contents = new[]
                {
            new { parts = new[] { new { text = prompt } } } //prompt api formatına sokuluyor.
        }
            };

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var response = await httpClient.PostAsJsonAsync(url, requestObject, serializeOptions);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);

                var aiText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                customerData.AiAnalysis = aiText;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                customerData.AiAnalysis = $"Hata Oluştu! Durum Kodu: {response.StatusCode}. Detay: {errorDetails}";
            }

            return customerData;
        }

        public SentimentAnalysisMainCoverTableDto GetCustomerInformations(int id)
        {
            var customer = _uow.Customers.GetFirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
                return new SentimentAnalysisMainCoverTableDto();


            return new SentimentAnalysisMainCoverTableDto
            {
                CustomerFullName = customer.CustomerName + " " + customer.CustomerSurname,
                CustomerEmail = customer.CustomerEmail,
                CustomerImage = customer.CustomerImageUrl,
                CustomerDescription = customer.CustomerDescription ?? "Açıklama Bulunamadı"
            };
        }

        public SentimentAnalysisStatisticsDto GetCustomerStatistics(int id)
        {
            var orders = _uow.Orders.GetQueryable()
                .Where(o => o.CustomerId == id);

            var customer = _uow.Customers.GetFirstOrDefault(c=>c.CustomerId == id);

            var reviewCount = _uow.Reviews.GetQueryable()
                .Count(r=>r.CustomerId == id);

            if (customer == null)
                return new SentimentAnalysisStatisticsDto();

            return new SentimentAnalysisStatisticsDto
            {
                CustomerTotalOrderCount = orders.Count(),
                CustomerCompletedOrderCount = orders.Count(o => o.OrderStatus == "Tamamlandı"),
                CustomerCancelledOrderCount = orders.Count(o => o.OrderStatus == "İptal Edildi"),
                CustomerCountryCityInformation = customer.CustomerCountry + "-" + customer.CustomerCity,
                CustomerReviewCount = reviewCount,
                CustomerTotalPurchase =  orders.Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice)
            };
        }
    }
}
