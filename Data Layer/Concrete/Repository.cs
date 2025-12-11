using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Abstract;
using Data_Layer.Context;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BigDataOrdersDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(BigDataOrdersDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public int GetCount(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public void Delete(int id)
        {
            T entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }

        public (List<T> items, int totalCount) GetAllWithPaging(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            int totalCount = _dbSet.Count();
            int skipCount = (pageNumber - 1) * pageSize;

            var query = GetAllIncluding(includeProperties);

            var items = query
            .OrderBy(e => EF.Property<object>(e, "Id"))
            .Skip(skipCount)
            .Take(pageSize)
            .ToList();

            return (items, totalCount);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public int GetDistinctCount<TKey>(Expression<Func<T, TKey>> selector)
        {
            IQueryable<T> query = _dbSet;

            return query.Select(selector).Distinct().Count();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return _dbSet.Sum(selector);
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return _dbSet.Sum(selector);
        }
    }
}
