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
        private readonly IProductRepository _productRepository;

        public ProductManager(IUnitOfWork uow, IProductRepository productRepository)
        {
            _uow = uow;
            _productRepository = productRepository;
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

        public Product GetFirstOrDefault(int id)
        {
            return _uow.Products.GetFirstOrDefault(p => p.ProductId == id);
        }

        public (List<Product> products, int totalCount) GetProductsForPaging(int pageNumber, int pageSize)
        {
            return _productRepository.GetProductsWithPaging(pageNumber, pageSize);
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
