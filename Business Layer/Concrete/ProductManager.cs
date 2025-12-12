using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
using Data_Layer.Abstract;
using Data_Layer.Concrete;
using Entity_Layer;

namespace Business_Layer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _uow;

        public ProductManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Product product)
        {
            _uow.Products.Add(product);
            _uow.Save();
        }

        public int CountProducts()
        {
            return _uow.Products.GetCount();
        }


        public void Delete(int id)
        {
            _uow.Products.Delete(id);
            _uow.Save();
        }

        public List<Product> GetAll()
        {
            return _uow.Products.GetAll().ToList();
        }
        public Product GetById(int id)
        {
            return _uow.Products.GetById(id);
        }

        public string GetCountryMostProducesProduct()
        {
            IQueryable<Product> query = _uow.Products.GetQueryable();

            var result = query
                .GroupBy(p=>p.CountryOfOrigin)
                .Select(g => new
                {
                    Country = g.Key,
                    ProductCount = g.Count()
                })
                .OrderByDescending(x => x.ProductCount)
                .FirstOrDefault();

            return result?.Country ?? "Bulunamadı";
        }

        public Product GetFirstOrDefault(int id)
        {
            return _uow.Products.GetFirstOrDefault(p => p.ProductId == id);
        }

        public string GetLeastStockedProductName()
        {
            IQueryable<Product> query = _uow.Products.GetQueryable();

            var leastStockedProduct = query
                .OrderBy(p => p.StockQuantity)
                .FirstOrDefault();

            return leastStockedProduct?.ProductName ?? "Bulunamadı";
        }

        public string GetMostExpensiveProductName()
        {
            IQueryable<Product> query = _uow.Products.GetQueryable();

            var mostExpensiveProduct = query.OrderByDescending(p => p.UnitPrice)
                .FirstOrDefault();

            return mostExpensiveProduct != null ? mostExpensiveProduct.ProductName : "Bulunamadı";
        }

        public (List<Product> products, int totalCount) GetProductsForPaging(int pageNumber, int pageSize)
        {
            return _uow.Products.GetAllWithPaging(pageNumber, pageSize,x=>x.Category);
        }

        public int GetTotalProductStock()
        {
            return _uow.Products.Sum(p => p.StockQuantity);
        }

        public void Update(Product product)
        {
            if (product.ProductId <= 0)
            {
                throw new Exception("Geçersiz ürün ID'si.");
            }

            _uow.Products.Update(product);
            _uow.Save();
        }
    }
}
