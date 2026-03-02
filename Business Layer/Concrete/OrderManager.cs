using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
using Business_Layer.DTOs;
using Core_Layer.Helpers;
using Data_Layer.Abstract;
using Entity_Layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace Business_Layer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _uow;


        public OrderManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Order order)
        {
            _uow.Orders.Add(order);
            _uow.Save();
        }

        public KpiCartsDto CompareTodayAndYesterdayOrdersForKpiCarts()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var today = new DateTime(2024, 12, 31);
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);

            var summary = query
                .Where(o => o.OrderDate >= yesterday && o.OrderDate < tomorrow)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    OrdersCount = g.Count(),
                    CompletedOrdersCount = g.Count(o => o.OrderStatus == "Tamamlandı"),
                    OrdersPrice = g.Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0M
                })
                .ToList();

            var todayData = summary.FirstOrDefault(s => s.Date == today);
            var yesterdayData = summary.FirstOrDefault(s => s.Date == yesterday);

            decimal tPrice = todayData?.OrdersPrice ?? 0;
            decimal yPrice = yesterdayData?.OrdersPrice ?? 0;
            int tCount = todayData?.OrdersCount ?? 0;
            int yCount = yesterdayData?.OrdersCount ?? 0;

            int tCompletedCount = todayData?.CompletedOrdersCount ?? 0;
            int yCompletedCount = yesterdayData?.CompletedOrdersCount ?? 0;

            decimal tAvg = tCount > 0 ? tPrice / tCount : 0;
            decimal yAvg = yCount > 0 ? yPrice / yCount : 0;

            // Yüzde değişim hesaplama fonksiyonu
            double CalcChange(decimal cur, decimal prev) => prev == 0 ? 0 : (double)((cur - prev) / prev * 100);

            return new KpiCartsDto
            {
                TodayOrdersCount = tCount,
                YesterdayOrdersCount = yCount,
                TodayCompletedOrdersCount = tCompletedCount,
                YesterdayCompletedOrdersCount = yCompletedCount,
                TodayOrdersPrice = (double)tPrice,
                YesterdayOrdersPrice = (double)yPrice,
                TodayOrdersAveragePrice = (double)tAvg,
                YesterdayOrdersAveragePrice = (double)yAvg,
                OrdersCountPercentageChange = CalcChange(tCount, yCount),
                CompletedOrdersCountPercentageChange = CalcChange(tCompletedCount, yCompletedCount),
                OrdersPricePercentageChange = CalcChange(tPrice, yPrice),
                OrdersAveragePricePercentageChange = CalcChange(tAvg, yAvg)
            };
        }

        public int CountCancelledOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderStatus == "İptal Edildi");
            //SELECT Count(*) FROM Orders WHERE OrderStatus='İptal Edildi'
        }

        public int CountCompletedOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderStatus == "Tamamlandı");
            //SELECT Count(*) FROM Orders WHERE OrderStatus='Tamamlandı'
        }

        public int CountOrders()
        {
            return _uow.Orders.GetCount();
            //SELECT Count(*) FROM Orders
        }

        public void Delete(int id)
        {
            _uow.Orders.Delete(id);
            _uow.Save();
            //DELETE * FROM Orders WHERE OrderId = id
        }

        public List<Order> GetAll()
        {
            return _uow.Orders.GetAll().ToList();
            //SELECT * FROM Orders
        }

        public decimal GetAverageRevenue()
        {
            return _uow.Orders.Average(o => (o.Quantity) * (o.Product.UnitPrice));
            //SELECT AVG(Quantity * UnitPrice) FROM Orders
        }

        public Order GetById(int id)
        {
            return _uow.Orders.GetById(id);
            //SELECT * FROM Orders WHERE OrderId = id
        }

        public List<CountryReportDto> GetCountryReportForMap() //Dashboard harita
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var start2023 = new DateTime(2023, 1, 1);
            var start2024 = new DateTime(2024, 1, 1);
            var start2025 = new DateTime(2025, 1, 1);

            var orders2023 = query
                .Where(o => o.OrderDate >= start2023 && o.OrderDate < start2024)
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new
                {
                    Country = g.Key,
                    Total2023 = g.Count()
                });

            var orders2024 = query
                .Where(o => o.OrderDate >= start2024 && o.OrderDate < start2025)
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new
                {
                    Country = g.Key,
                    Total2024 = g.Count()
                });

            var rawData = orders2023
                .Join(orders2024,
                y2023 => y2023.Country,
                y2024 => y2024.Country,
                (y2023, y2024) => new
                {
                    Country = y2023.Country,
                    Total2023 = y2023.Total2023,
                    Total2024 = y2024.Total2024
                })
            .ToList();

            var result = rawData
                .Select(x => new CountryReportDto
                {
                    Country = x.Country,
                    Total2023 = x.Total2023,
                    Total2024 = x.Total2024,
                    ChangeRate = x.Total2023 == 0
                        ? 0
                        : ((decimal)(x.Total2024 - x.Total2023) / x.Total2023) * 100,

                    Latitude = CountryCoordinateHelper.GetLatitude(x.Country) ?? 0,
                    Longitude = CountryCoordinateHelper.GetLongitude(x.Country) ?? 0
                })
                .Where(x => x.Latitude != 0 && x.Longitude != 0)
                .ToList();

            return result;
        }

        public Order GetFirstOrDefault(int id)
        {
            return _uow.Orders.GetFirstOrDefault(o => o.OrderId == id);
            //SELECT TOP 1 * FROM Orders WHERE OrderId = id
        }

        public List<TodayOrdersDto> GetLast10OrdersToday()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var date = new DateTime(2024, 12, 31);
            var last10OrdersToday = query
                .Where(o => o.OrderDate.Date == date)
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Take(5)
                .Select(o => new TodayOrdersDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.CustomerName + " " + o.Customer.CustomerSurname,
                    ProductName = o.Product.ProductName,
                    UnitPrice = o.Product.UnitPrice,
                    TotalPrice = o.Quantity * o.Product.UnitPrice,
                    Quantity = o.Quantity,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod
                });

            return last10OrdersToday.ToList();
        } //Dashboard en alttaki kısım

        public string GetLeastOrderedProduct() //TextualStatistics 12. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var leastOrderedProduct = query
                .GroupBy(o => o.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.OrderCount)
                .FirstOrDefault();

            return leastOrderedProduct == null ? "Bulunamadı" : $"{leastOrderedProduct.ProductName} ({leastOrderedProduct.OrderCount} adet)";
        }

        public string GetMostCancelledProduct() //TextualStatistics 8. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostCancelledProduct = query
                .Where(p => p.OrderStatus == "İptal Edildi")
                .GroupBy(p => p.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    CancelCount = g.Count()
                })
                .OrderByDescending(x => x.CancelCount)
                .FirstOrDefault();

            return mostCancelledProduct == null ? "Bulunamadı" : $"{mostCancelledProduct.ProductName} ({mostCancelledProduct.CancelCount} adet)";
        }

        public string GetMostCompletedProductName() //TextualStatistics 7. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostCompletedProduct = query
                .Where(o => o.OrderStatus == "Tamamlandı")
                .GroupBy(o => o.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    CompletedCount = g.Count()
                })
                .OrderByDescending(x => x.CompletedCount)
                .FirstOrDefault();

            return mostCompletedProduct == null ? "Bulunamadı" : $"{mostCompletedProduct.ProductName} ({mostCompletedProduct.CompletedCount} adet)";
        }

        public string GetMostOrderedCategory() //TextualStatistics 14. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCategory = query
                .GroupBy(o => o.Product.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCategory == null ? "Bulunamadı" : $"{mostOrderedCategory.CategoryName} ({mostOrderedCategory.OrderCount} adet)";
        }

        public string GetMostOrderedCity() //TextualStatistics 4. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCity = query
                .GroupBy(o => o.Customer.CustomerCity)
                .Select(g => new
                {
                    City = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCity == null ? "Bulunamadı" : $"{mostOrderedCity.City} ({mostOrderedCity.OrderCount} adet)";
        }

        public string GetMostOrderedCountry() //TextualStatistics 3. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCountry = query
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new
                {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCountry == null ? "Bulunamadı" : $"{mostOrderedCountry.Country} ({mostOrderedCountry.OrderCount} adet)";
        }

        public string GetMostOrderedCustomer() //TextualStatistics 15. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCustomer = query
                .GroupBy(o => new { o.Customer.CustomerName, o.Customer.CustomerSurname })
                .Select(g => new
                {
                    FullName = g.Key.CustomerName + " " + g.Key.CustomerSurname,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCustomer == null ? "Bulunamadı" : $"{mostOrderedCustomer.FullName} ({mostOrderedCustomer.OrderCount} adet)";
        }

        public string GetMostOrderedPayment() //TextualStatistics 13. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedPayment = query
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedPayment == null ? "Bulunamadı" : $"{mostOrderedPayment.PaymentMethod} ({mostOrderedPayment.OrderCount} adet)";
        }

        public string GetMostOrderedProduct() //TextualStatistics 11. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedProduct = query
                .GroupBy(o => o.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedProduct == null ? "Bulunamadı" : $"{mostOrderedProduct.ProductName} ({mostOrderedProduct.OrderCount} adet)";
        }

        public string GetMostOrderedProductThisMonth() //TextualStatistics 16. Kart
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedProduct = query
                .Where(p => p.OrderDate.Month == 11 && p.OrderDate.Year == 2025)
                .GroupBy(p => p.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedProduct == null ? "Bulunamadı" : $"{mostOrderedProduct.ProductName} ({mostOrderedProduct.OrderCount} adet)";
        }

        public List<OrderStatusChartDto> GetOrderStatusChartData() //Dashboard sayfasındaki en üstteki bar'lar
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var allStatuses = new[] { "Tamamlandı", "İptal Edildi", "Hazırlanıyor", "Kargoda", "Beklemede" };

            var totalOrders = query.Count();

            var statusCounts = query
                .GroupBy(o => o.OrderStatus)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var result = allStatuses.Select(status =>
            {
                var data = statusCounts.FirstOrDefault(x => x.Status == status);

                var count = data?.Count ?? 0;

                return new OrderStatusChartDto
                {
                    Status = status,
                    Count = count,
                    Rate = totalOrders == 0 ? 0 : (double)count / totalOrders * 100
                };
            })
            .ToList();

            return result;
        }

        public (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize)
        {
            return _uow.Orders.GetAllWithPaging(pageNumber, pageSize, o => o.Customer, o => o.Product);
        }

        public int GetThisYearOrders() //NumericalStatistics 10. Kart
        {
            return _uow.Orders.GetCount(o => o.OrderDate.Year == DateTime.Now.Year);

            //return _uow.Orders.GetCount(o=>o.OrderDate >= new DateTime(2025,01,01) && o.OrderDate <= new DateTime(2025,12,31));
        }

        public decimal GetTotalRevenue() //NumericalStatistics 11. Kart
        {
            return _uow.Orders.Sum(o => ((o.Quantity) * (o.Product.UnitPrice)));
        }

        public List<MonthlySalesDto> Last6MonthsSalesGraph()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var today = new DateTime(2025, 12, 31);
            var sixMonthsAgo = new DateTime(today.AddMonths(-5).Year, today.AddMonths(-5).Month, 1); // yılı ve ayı alır. Ardından bulunan ayın en başına gider ki tüm ay kapsansın.

            return query
                .Where(o=>o.OrderDate <=today && o.OrderDate >= sixMonthsAgo)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month }) //farklı yılların ayları karışmaması için böyle yapıldı.
                .OrderBy(g=>g.Key.Year)
                .ThenBy(g=>g.Key.Month)
                .Select(g => new MonthlySalesDto
                {
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                    CompletedCount = g.Count(o=>o.OrderStatus=="Tamamlandı"),
                    CompletedAmount = g.Where(o => o.OrderStatus == "Tamamlandı").Sum(o=> (decimal?)(o.Quantity*o.Product.UnitPrice)) ?? 0,
                    CancelledCount = g.Count(o => o.OrderStatus == "İptal Edildi"),
                    CancelledAmount = g.Where(o => o.OrderStatus == "İptal Edildi").Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0,
                    ShippedCount = g.Count(o => o.OrderStatus == "Kargoda"),
                    ShippedAmount = g.Where(o => o.OrderStatus == "Kargoda").Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0
                })
                .ToList();
        }

        public MainChartDto TodaysSalesStatus() 
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var todayStart = new DateTime(2024, 12, 31);
            var tomorrowStart = todayStart.AddDays(1);

            return query
                .Where(o => o.OrderDate >= todayStart && o.OrderDate < tomorrowStart)
                .GroupBy(o => 1)
                .Select(g => new MainChartDto
                {
                    CompletedOrdersPrice = g.Where(o => o.OrderStatus == "Tamamlandı")
                                        .Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0,

                    CancelledOrdersPrice = g.Where(o => o.OrderStatus == "İptal Edildi")
                                                    .Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0,

                    ShippingOrdersPrice = g.Where(o => o.OrderStatus == "Kargoda").Sum(o => (decimal?)(o.Quantity * o.Product.UnitPrice)) ?? 0

                }).FirstOrDefault() ?? new MainChartDto();

        }
        public void Update(Order order)
        {
            _uow.Orders.Update(order);
            _uow.Save();
        }
    }
}
