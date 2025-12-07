using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Data_Layer.Abstract
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, Object>>[] includeProperties);
        T GetById (int id);
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add (T entity);
        void Update (T entity);
        void Delete (int id);
        int Count();

    }
}
