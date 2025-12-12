using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product GetFirstOrDefault(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        (List<Product> products, int totalCount) GetProductsForPaging(int pageNumber, int pageSize);
        int CountProducts();
        int GetTotalProductStock();
        string GetMostExpensiveProductName();
        string GetLeastStockedProductName();
    }
}

