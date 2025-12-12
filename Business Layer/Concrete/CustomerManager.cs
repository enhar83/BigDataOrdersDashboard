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
    public class CustomerManager : ICustomerService
    {
        private readonly IUnitOfWork _uow;

        public CustomerManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public void Add(Customer customer)
        {
            _uow.Customers.Add(customer);
            _uow.Save();
        }

        public int CountCustomers()
        {
            return _uow.Customers.GetCount();
        }

        public void Delete(int id)
        {
            _uow.Customers.Delete(id);
            _uow.Save();
        }

        public List<Customer> GetAll()
        {
            return _uow.Customers.GetAll().ToList();
        }

        public Customer GetById(int id)
        {
            return _uow.Customers.GetById(id);
        }

        public int GetCityNumber()
        {
            return _uow.Customers.GetDistinctCount(c => c.CustomerCity);
        }

        public int GetCountryNumber()
        {
            return _uow.Customers.GetDistinctCount(c => c.CustomerCountry);
        }

        public (List<Customer> customers, int totalCount) GetCustomersWithPaging(int pageNumber, int pageSize)
        {
            return _uow.Customers.GetAllWithPaging(pageNumber, pageSize);
        }

        public string GetFirstAddedCustomer()
        {
            IQueryable<Customer> query = _uow.Customers.GetQueryable();

            var firstCustomer=query
                .OrderBy(c => c.CustomerId)
                .FirstOrDefault();

            if (firstCustomer == null)
                return "Bulunamadı";

            return $"{firstCustomer.CustomerName} {firstCustomer.CustomerSurname}";
        }

        public Customer GetFirstOrDefault(int id)
        {
            return _uow.Customers.GetFirstOrDefault(c => c.CustomerId == id);
        }

        public string GetLastAddedCustomer()
        {
            IQueryable<Customer> query = _uow.Customers.GetQueryable();

            var lastCustomer = query
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefault();

            if (lastCustomer == null)
                return "Bulunamadı";

            return $"{lastCustomer.CustomerName} {lastCustomer.CustomerSurname}";
        }

        public void Update(Customer customer)
        {
            if (customer.CustomerId <= 0)
            {
                throw new Exception("Geçersiz müşteri ID'si.");
            }

            _uow.Customers.Update(customer);
            _uow.Save();
        }
    }
}
