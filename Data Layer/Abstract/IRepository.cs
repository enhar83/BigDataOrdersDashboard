using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Data_Layer.Abstract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, Object>>[] includeProperties);
        T GetById(int id);
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        (List <T> items, int totalCount) GetAllWithPaging(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includeProperties);
        int GetCount(Expression<Func<T, bool>> filter = null);
        int GetDistinctCount<TKey>(Expression<Func<T, TKey>> selector);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null);
        decimal Sum(Expression<Func<T, decimal>> selector);
        int Sum(Expression<Func<T, int>> selector);
    }
}

//Sum metodlarının tür ayırt etmeyecek şekilde yapamamızın sebebi IQueryable arayüzünün generic tip parametresinin tek bir türle sınırlı olmamasıdır.
//Bu nedenle, farklı türlerdeki toplamları hesaplamak için ayrı metotlar tanımlanmıştır.