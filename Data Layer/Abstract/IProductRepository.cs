using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Data_Layer.Abstract
{
    public interface IProductRepository: IRepository<Product>
    {
        (List<Product> products, int totalCount) GetProductsWithPaging(int pageNumber, int pageSize);
    }
}
