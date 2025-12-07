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
        private readonly ICustomerRepository _customerRepository;
        public CustomerManager(IUnitOfWork uow, ICustomerRepository customerRepository)
        {
            _uow = uow;
            _customerRepository = customerRepository;
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

        public (List<Customer> customers, int totalCount) GetCustomersWithPaging(int pageNumber, int pageSize)
        {
            return _customerRepository.GetCustomersWithPaging(pageNumber, pageSize);
        }

        public Customer GetFirstOrDefault(int id)
        {
            return _uow.Customers.GetFirstOrDefault(c => c.CustomerId == id);
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
