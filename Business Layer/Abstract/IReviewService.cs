using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface IReviewService
    {
        List<Review> GetAll();
        Review GetById(int id);
        Review GetFirstOrDefault(int id);
        void Add(Review review);
        void Update(Review review);
        void Delete(int id);
        (List<Review> reviews, int totalCount) GetReviewsWithPaging(int pageNumber, int pageSize);
        int ReviewCount();
    }
}
