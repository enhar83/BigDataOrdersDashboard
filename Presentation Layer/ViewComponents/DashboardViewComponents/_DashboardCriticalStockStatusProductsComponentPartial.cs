using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardCriticalStockStatusProductsComponentPartial:ViewComponent
    {
        private readonly IProductService _productService;

        public _DashboardCriticalStockStatusProductsComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        { 
            return View();
        }
    }
}
