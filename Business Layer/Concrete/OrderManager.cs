using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
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

        public int CountCancelledOrders()
        {
            return _uow.Orders.GetCount(o=> o.OrderStatus=="İptal Edildi");
        }

        public int CountCompletedOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderStatus=="Tamamlandı");
        }

        public int CountOrders()
        {
            return _uow.Orders.GetCount();
        }

        public void Delete(int id)
        {
            _uow.Orders.Delete(id);
            _uow.Save();
        }

        public List<Order> GetAll()
        {
            return _uow.Orders.GetAll().ToList();
        }

        public decimal GetAverageRevenue()
        {
            return _uow.Orders.Average(o => (o.Quantity) * (o.Product.UnitPrice));
        }

        public Order GetById(int id)
        {
            return _uow.Orders.GetById(id);
        }

        public Order GetFirstOrDefault(int id)
        {
            return _uow.Orders.GetFirstOrDefault(o => o.OrderId == id);
        }

        public string GetMostCancelledProduct()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostCancelledProduct = query
                .Where(p=>p.OrderStatus=="İptal Edildi")
                .GroupBy(p => p.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    CancelCount = g.Count()
                })
                .OrderByDescending(x => x.CancelCount)
                .FirstOrDefault();

            return mostCancelledProduct?.ProductName ?? "Bulunamadı";
        }

        public string GetMostOrderedCategory()
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

            return mostOrderedCategory?.CategoryName ?? "Bulunamadı";
        }

        public string GetMostOrderedCity()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCity=query
                .GroupBy(o => o.Customer.CustomerCity)
                .Select(g => new
                {
                    City = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCity?.City ?? "Bulunamadı";
        }

        public string GetMostOrderedCountry()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCity = query
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new
                {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCity?.Country ?? "Bulunamadı";
        }

        public string GetMostOrderedCustomer()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedCustomer = query
                .GroupBy(o => new { o.Customer.CustomerName, o.Customer.CustomerSurname})
                .Select(g => new
                {
                    FullName = g.Key.CustomerName + " " + g.Key.CustomerSurname,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedCustomer != null ? mostOrderedCustomer.FullName : string.Empty;
        }

        public string GetMostOrderedPayment()
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

            return mostOrderedPayment?.PaymentMethod ?? "Bulunamadı";
        }

        public string GetMostOrderedProductThisMonth()
        {
            IQueryable<Order> query = _uow.Orders.GetQueryable();

            var mostOrderedProduct = query
                .Where(p => p.OrderDate.Month == DateTime.Now.Month && p.OrderDate.Year == DateTime.Now.Year)
                .GroupBy(p => p.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return mostOrderedProduct?.ProductName ?? "Bulunamadı";
        }

        public (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize)
        {
            return _uow.Orders.GetAllWithPaging(pageNumber, pageSize,o=> o.Customer,o=>o.Product);
        }

        public int GetThisYearOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderDate.Year == DateTime.Now.Year);

            //return _uow.Orders.GetCount(o=>o.OrderDate >= new DateTime(2025,01,01) && o.OrderDate <= new DateTime(2025,12,31));
        }

        public decimal GetTotalRevenue()
        {


            return _uow.Orders.Sum(o => ((o.Quantity) * (o.Product.UnitPrice)));
        }

        public void Update(Order order)
        {
            _uow.Orders.Update(order);
            _uow.Save();
        }
    }
}
