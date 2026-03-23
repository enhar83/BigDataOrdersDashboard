using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Data_Layer.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Order> Orders { get; }
        IRepository<Review> Reviews { get; }
        IRepository<Entity_Layer.Message> Messages { get; }
        int Save();
    }
}
