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
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Product> Products { get; private set; }

        private readonly BigDataOrdersDbContext _db;

        public UnitOfWork(BigDataOrdersDbContext db)
        {
            _db = db;
            
            Categories = new Repository<Category>(_db);
            Products = new Repository<Product>(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}
