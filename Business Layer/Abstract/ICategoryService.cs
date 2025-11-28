using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}

//Controller'ın doğrudan managera bağımlı olmasını engeller.
//DI düzgün çalışmasını ve DIP'ı sağlar.
