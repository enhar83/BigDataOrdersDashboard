using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation_Layer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult ProductList(int page=1)
        {
            int pageSize = 12;

            var (values, totalCount) = _productService.GetProductsForPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(values);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var categoryList = _categoryService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            ViewBag.CategoryList = categoryList;

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _productService.Add(product);
            return RedirectToAction("ProductList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            _productService.Delete(id);
            return RedirectToAction("ProductList");
        }
    }
}
