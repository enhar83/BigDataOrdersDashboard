using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
using Data_Layer.Abstract;

namespace Business_Layer.Concrete
{
    public class ReviewManager:IReviewService
    {
        private readonly IUnitOfWork _uow;

        public ReviewManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}
