using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace Business_Layer.MachineLearning.Concrete
{
    public class CustomerAnalyticsManager : ICustomerAnalyticsService
    {
        private readonly IUnitOfWork _uow;

        public CustomerAnalyticsManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public CustomerAnalyticsMainStatisticsDto GetCustomerAnalyticsMainStatistics()
        {
            var totalCustomerCount = _uow.Customers.GetCount();
            if (totalCustomerCount == 0) return new CustomerAnalyticsMainStatisticsDto();

            var orderCount = _uow.Orders.GetCount();
            var averageOrderCountPerPerson=(double)orderCount/(double)totalCustomerCount;

            var mostActiveCity=_uow.Orders.GetQueryable()
                .GroupBy(o=> o.Customer.CustomerCity)
                .OrderByDescending(g=>g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Veri Yok";

            var today = new DateTime(2025, 12, 31);
            var activeCustomerCountForLast3Days = _uow.Orders.GetQueryable()
                .Where(o => o.OrderDate >= today.AddDays(-3) && o.OrderDate <= today)
                .Select(o=>o.CustomerId) //sadece customerId kolonu seçildiği için distinc uygulanır.
                .Distinct().Count();

            return new CustomerAnalyticsMainStatisticsDto
            {
                TotalCustomerCount = totalCustomerCount,
                AverageOrderCountPerPerson = (int)Math.Round((double)orderCount / totalCustomerCount),
                MostActiveCity = mostActiveCity,
                ActiveCustomerCountForLast3Days= activeCustomerCountForLast3Days,
            };
        }
    }
}
