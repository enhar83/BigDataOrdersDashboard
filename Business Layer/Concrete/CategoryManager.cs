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
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Category category)
        {
            if(string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new Exception("Kategori adı boş olamaz.");
            }

            _uow.Categories.Add(category);
            _uow.Save();
        }

        public void Delete(int id)
        {
            _uow.Categories.Delete(id);
            _uow.Save();
        }

        public List<Category> GetAll()
        {
            return _uow.Categories.GetAll().ToList();
        }

        public Category GetById(int id)
        {
            return _uow.Categories.GetById(id);
        }

        public Category GetFirstOrDefault(int id)
        {
            return _uow.Categories.GetFirstOrDefault(c=>c.CategoryId==id);
        }

        public void Update(Category category)
        {
            if (category.CategoryId <= 0)
            {
                throw new Exception("Geçersiz kategori ID'si.");
            }

            _uow.Categories.Update(category);
            _uow.Save();
        }
    }
}
