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

        public List<CustomerLoyaltyScoreDto> GetCustomerLoyaltyScores()
        {
            var date = new DateTime(2025, 12, 31);

            var rawData = _uow.Customers.GetQueryable()
                .Where(c => c.CustomerCountry == "İtalya")
                .Select(g => new
                {
                    CustomerName = g.CustomerName + " " + g.CustomerSurname,
                    OrderCount = g.Orders.Count(),
                    TotalSpend = g.Orders.Sum(o => (double)o.Quantity * (double)o.Product.UnitPrice),
                    LastOrderDate = g.Orders.Max(o => (DateTime?)o.OrderDate)
                })
                .ToList();

            return rawData.Select(x =>
            {
                var daySinceLastOrder = (x.LastOrderDate.HasValue)
                    ? (date - x.LastOrderDate.Value).TotalDays
                    : 999;

                double recenyScore = daySinceLastOrder switch
                {
                    <= 2 => 100, 
                    <= 4 => 85,
                    <= 8 => 70,
                    <= 20 => 50,
                    <= 30 => 20,
                    _ => 0 
                };

                double frequencyScore = x.OrderCount switch
                {
                    >= 500 => 100,
                    >= 300 => 85,
                    >= 200 => 65,
                    >= 100 => 40,
                    >= 50 => 20,
                    _ => 5
                };

                double monetaryScore = x.TotalSpend switch
                {
                    >= 500000 => 100,
                    >= 250000 => 85,
                    >= 100000 => 70,
                    >= 50000 => 50,
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
    }
}
