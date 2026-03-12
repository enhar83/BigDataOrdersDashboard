using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Entity_Layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Packaging.Signing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business_Layer.MachineLearning.Concrete
{
    public class CustomerAnalyticsManager : ICustomerAnalyticsService
    {
        private readonly IUnitOfWork _uow;

        public CustomerAnalyticsManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<CustomerCityOrderCountForDonutDto> GetCityOrderCountForDonut()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var totalCount = query.Count();

            return query
                .GroupBy(o => new { o.Customer.CustomerCity, o.Customer.CustomerCountry })
                .Select(g=>new CustomerCityOrderCountForDonutDto
                {
                    CityName = g.Key.CustomerCity,
                    CountryName = g.Key.CustomerCountry,
                    OrderCount = g.Count(),
                    Percentage = totalCount == 0 ? 0 : (double)g.Count() / totalCount * 100
                })
                .OrderByDescending(o => o.OrderCount)
                .Take(5)
                .ToList();
        }

        public CustomerAnalyticsMainStatisticsDto GetCustomerAnalyticsMainStatistics()
        {
            var totalCustomerCount = _uow.Customers.GetCount();
            if (totalCustomerCount == 0) return new CustomerAnalyticsMainStatisticsDto();

            var orderCount = _uow.Orders.GetCount();
            var averageOrderCountPerPerson = (double)orderCount / (double)totalCustomerCount;

            var mostActiveCity = _uow.Orders.GetQueryable()
                .GroupBy(o => o.Customer.CustomerCity)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Veri Yok";

            var today = new DateTime(2025, 12, 31);
            var activeCustomerCountForLast3Days = _uow.Orders.GetQueryable()
                .Where(o => o.OrderDate >= today.AddDays(-3) && o.OrderDate <= today)
                .Select(o => o.CustomerId) //sadece customerId kolonu seçildiği için distinct uygulanır.
                .Distinct().Count();

            return new CustomerAnalyticsMainStatisticsDto
            {
                TotalCustomerCount = totalCustomerCount,
                AverageOrderCountPerPerson = (int)Math.Round((double)orderCount / totalCustomerCount),
                MostActiveCity = mostActiveCity,
                ActiveCustomerCountForLast3Days = activeCustomerCountForLast3Days,
            };
        }

        public CustomerAnalyticsSubstatisticsDto GetCustomerAnalyticsSubstatistics()
        {
            var date = new DateTime(2024,12,31);
            var lastMonth = date.AddMonths(-1);
            var twoMonthsAgo = date.AddMonths(-2);

            var customerStats = _uow.Orders.GetQueryable()
                .GroupBy(o => new { o.CustomerId ,o.Customer.CustomerName, o.Customer.CustomerSurname })
                .Select(g => new {
                    FullName = g.Key.CustomerName + " " + g.Key.CustomerSurname,
                    TotalCount = g.Count(),
                    LastMonthCount = g.Count(o => o.OrderDate >= lastMonth && o.OrderDate <= date),
                    TwoMonthAgoCount = g.Count(o => o.OrderDate >= twoMonthsAgo && o.OrderDate <= lastMonth),
                })
                .ToList(); 

            return new CustomerAnalyticsSubstatisticsDto
            {
                MostActiveCustomerName = customerStats.OrderByDescending(x => x.TotalCount).FirstOrDefault()?.FullName ?? "Veri Yok",
                MostActiveCustomerOrderCount = customerStats.Max(x => x.TotalCount),

                LeastActiveCustomerName = customerStats.OrderBy(x => x.TotalCount).FirstOrDefault()?.FullName ?? "Veri Yok",
                LeastActiveCustomerOrderCount = customerStats.Min(x => x.TotalCount),

                RisingStarCustomerName = customerStats.OrderByDescending(x => x.LastMonthCount).FirstOrDefault()?.FullName ?? "Veri Yok",
                RisingStarCustomerOrderCount = customerStats.Max(x => x.LastMonthCount),

                FallingStarCustomerName = customerStats.Where(x => x.LastMonthCount > 0).OrderBy(x => x.LastMonthCount).FirstOrDefault()?.FullName ?? "Veri Yok",
                FallingStarCustomerOrderCount = customerStats.Where(x => x.LastMonthCount > 0).Min(x => x.LastMonthCount)
            };
        }

        public List<CustomerSegmentForChartDto> GetCustomerSegmentForChart()
        {
            var customerStats = _uow.Orders.GetQueryable()
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalSpend = g.Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice)
                })
                .ToList();

            //RFM = Recency (Yenilik), Frequency(Sıklık), Monetary(Parasal Değer). Burada F ve R veri setinden dolayı kullanılamadı. Sadece M kullanıldı. 
            var vipCustomers = customerStats.Where(c => c.TotalSpend > 800000).ToList();
            var regularCustomers = customerStats.Where(c => c.TotalSpend <= 800000 && c.TotalSpend > 650000).ToList();
            var lowVolumeCustomers = customerStats.Where(c => c.TotalSpend <= 650000).ToList();

            return new List<CustomerSegmentForChartDto>
            {
                new CustomerSegmentForChartDto { SegmentName = "V.I.P Müşteriler", Count = vipCustomers.Count, AverageSegmentSpend = vipCustomers.Any() ? vipCustomers.Average(x => x.TotalSpend) : 0 },
                new CustomerSegmentForChartDto { SegmentName = "Düzenli Alıcılar", Count = regularCustomers.Count,AverageSegmentSpend = regularCustomers.Any() ? regularCustomers.Average(x => x.TotalSpend) : 0 },
                new CustomerSegmentForChartDto { SegmentName = "Potansiyel Büyüme", Count = lowVolumeCustomers.Count,AverageSegmentSpend = lowVolumeCustomers.Any() ? lowVolumeCustomers.Average(x => x.TotalSpend) : 0 }
            };
        }
    }
}
