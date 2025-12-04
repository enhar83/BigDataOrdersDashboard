using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Data_Layer.Abstract
{
    public interface IOrderRepository: IRepository<Order>
    {
        (List<Order>, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize);
    }
}
