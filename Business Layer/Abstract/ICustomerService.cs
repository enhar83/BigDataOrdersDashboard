using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Customer GetFirstOrDefault(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        (List<Customer> customers, int totalCount) GetCustomersWithPaging(int pageNumber, int pageSize);
        int CountCustomers();
        int GetCountryNumber();
        int GetCityNumber();
    }
}
