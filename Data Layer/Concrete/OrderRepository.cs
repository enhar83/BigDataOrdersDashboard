using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Abstract;
using Data_Layer.Context;
using Entity_Layer;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Concrete
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(BigDataOrdersDbContext db) : base(db)
        {
        }

        public (List<Order>, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize)
        {
            int totalCount = _dbSet.Count();
            int skipCount = (pageNumber - 1) * pageSize;

            var orders = _dbSet
                .Include(o => o.Customer).Include(o => o.Product)
                .OrderBy(o => o.OrderId)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (orders, totalCount);
        }
    }
}
