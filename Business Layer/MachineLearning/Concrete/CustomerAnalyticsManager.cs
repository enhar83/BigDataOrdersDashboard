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

            var totalSpending = _uow.Orders.GetQueryable()
                .Include(o=>o.Product)
                .Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice);
            var totalSpendingPerPerson=totalSpending/totalCustomerCount;

            var mostActiveCity=_uow.Orders.GetQueryable()
                .GroupBy(o=> o.Customer.CustomerCity)
                .OrderByDescending(g=>g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Veri Yok";

            return new CustomerAnalyticsMainStatisticsDto
            {
                TotalCustomerCount = totalCustomerCount,
                AverageOrderCountPerPerson = (int)Math.Round((double)orderCount / totalCustomerCount),
                AverageSpending = totalSpendingPerPerson,
                MostActiveCity = mostActiveCity
            };
        }
    }
}
