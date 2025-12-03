using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Data_Layer.Abstract
{
    public interface ICustomerRepository:IRepository<Customer>
    {
        (List<Customer> customers, int totalCount) GetCustomersWithPaging( int pageNumber, int pageSize);
    }
}
