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

        public (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize)
        {
            return _orderRepository.GetOrdersWithPaging(pageNumber, pageSize);
        }

        public void Update(Order order)
        {
            _uow.Orders.Update(order);
            _uow.Save();
        }
    }
}
