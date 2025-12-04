using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order GetById(int id);
        Order GetFirstOrDefault(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize);
    }
}
