using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CategoryList()
        {
            var categories = _categoryService.GetAll();

            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _categoryService.Add(category);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var categoryToUpdate = _categoryService.GetFirstOrDefault(id);
            if (categoryToUpdate == null) return NotFound();
            return View(categoryToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCategory(Category updatedCategory)
        {
            if (updatedCategory == null || updatedCategory.CategoryId <= 0)
                return RedirectToAction("CategoryList");

            _categoryService.Update(updatedCategory);

            return RedirectToAction("CategoryList");

        }
    }
}
