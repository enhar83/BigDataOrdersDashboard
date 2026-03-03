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
    public class ReviewManager : IReviewService
    {
        private readonly IUnitOfWork _uow;

        public ReviewManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Review review)
        {
            _uow.Reviews.Add(review);
            _uow.Save();
        }

        public void Delete(int id)
        {
            _uow.Reviews.Delete(id);
            _uow.Save();
        }

        public List<Review> GetAll()
        {
            return _uow.Reviews.GetAll().ToList();
        }

        public Review GetById(int id)
        {
            return _uow.Reviews.GetById(id);
        }

        public Review GetFirstOrDefault(int id)
        {
            return _uow.Reviews.GetFirstOrDefault(r => r.ReviewId == id);
        }

        public (List<Review> reviews, int totalCount) GetReviewsWithPaging(int pageNumber, int pageSize)
        {
            return _uow.Reviews.GetAllWithPaging(pageNumber, pageSize);
        }

        public void Update(Review review)
        {
            _uow.Reviews.Update(review);
            _uow.Save();
        }
    }
}
