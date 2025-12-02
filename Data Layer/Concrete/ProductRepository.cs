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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BigDataOrdersDbContext db):base(db)
        {
            
        }

        public (List<Product> products, int totalCount) GetProductsWithPaging(int pageNumber, int pageSize)
        {
            int totalCount = _dbSet.Count();
            int skipCount = (pageNumber - 1) * pageSize;

            var products = _dbSet
                .Include(p=> p.Category)
                .OrderBy(p => p.ProductId)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (products, totalCount);
        }
    }
}
