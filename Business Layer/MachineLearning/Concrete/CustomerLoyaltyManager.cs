using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.MachineLearning.Abstract;
using Core_Layer.DTOs.DTOsForMachineLearning;
using Data_Layer.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.MachineLearning.Concrete
{
    public class CustomerLoyaltyManager : ICustomerLoyaltyService
    {
        private readonly IUnitOfWork _uow;

        public CustomerLoyaltyManager(IUnitOfWork uow)
        {
            _uow = uow;
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
