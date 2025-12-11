using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
using Data_Layer.Abstract;
using Entity_Layer;

namespace Business_Layer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IUnitOfWork uow, IOrderRepository orderRepository)
        {
            _uow = uow;
            _orderRepository = orderRepository;
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

        public Order GetById(int id)
        {
            return _uow.Orders.GetById(id);
        }

        public Order GetFirstOrDefault(int id)
        {
            return _uow.Orders.GetFirstOrDefault(o => o.OrderId == id);
        }

        public string GetMostOrderingCountry()
        {
            var query = _uow.Orders.GetQueryable();

            var result = query
                .GroupBy(o=> o.Customer.CustomerCountry)
                .Select(g=> new
                {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x=> x.OrderCount)
                .FirstOrDefault();

            return result?.Country ?? "Bulunamadı";
        }

        public string GetMostOrderingCustomer()
        {
            var query = _uow.Orders.GetQueryable();

            var result = query
                .GroupBy(o => new { o.Customer.CustomerName, o.Customer.CustomerSurname })
                .Select(g => new
                {
                    FullName = g.Key.CustomerName+ " " + g.Key.CustomerSurname,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            return result?.FullName ?? "Bulunamadı";
        }

        public int GetOctoberOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderDate.Month == 10);
        }

        public (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize)
        {
            return _orderRepository.GetOrdersWithPaging(pageNumber, pageSize);
        }

        public int GetThisYearOrders()
        {
            return _uow.Orders.GetCount(o => o.OrderDate.Year == DateTime.Now.Year);

            //return _uow.Orders.GetCount(o=>o.OrderDate >= new DateTime(2025,01,01) && o.OrderDate <= new DateTime(2025,12,31));
        }

        public void Update(Order order)
        {
            _uow.Orders.Update(order);
            _uow.Save();
        }
    }
}
