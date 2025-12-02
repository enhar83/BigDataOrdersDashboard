using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult ProductList(int page=1)
        {
            int pageSize = 12;

            var (values, totalCount) = _productService.GetProductsForPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;

            return View(values);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
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
