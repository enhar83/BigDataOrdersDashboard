using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Business_Layer.MachineLearning.Concrete
{
    public class SentimentAnalysisManager : ISentimentAnalysisService
    {
        private readonly IUnitOfWork _uow;

        //pooling: arka planda bağlantıları yönetir, ihtiyaç bittiğinde onları öldürmek yerine yeniden kullanmak üzere saklar.
        //apı anahtarlarını -, base url gibi ayarları program.cs içerisinde tek bir yerden yönetmeyi saülar.
        //ancak program.cs içerisinde builder.Services.AddHttpClient(); yazılması unutulmamalıdır.
        private readonly IHttpClientFactory _httpClientFactory;


        public SentimentAnalysisManager(IUnitOfWork uow, IHttpClientFactory httpClientFactory)
        {
            _uow = uow;
            _httpClientFactory = httpClientFactory;
        }

        public SentimentAnalysisMainCoverTableDto GetCustomerInformations(int id)
        {
            id = 8;
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
            id = 8;

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
