using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Abstract;
using Data_Layer.Context;
using Entity_Layer;

namespace Data_Layer.Concrete
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(BigDataOrdersDbContext db) : base(db)
        {
        }

        public (List<Customer> customers, int totalCount) GetCustomersWithPaging(int pageNumber, int pageSize)
        {
            int totalCount = _dbSet.Count();
            int skipCount = (pageNumber - 1) * pageSize;

            var customers =_dbSet
                .OrderBy(c => c.CustomerId)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (customers, totalCount);
        }
    }
}
