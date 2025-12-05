using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult OrderList(int page = 1)
        {
            int pageSize = 12;

            var (values, totalCount) = _orderService.GetOrdersWithPaging(page, pageSize);
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(values);
        }

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrder(Order order)
        {
            order.OrderDate = DateTime.Now;

            _orderService.Add(order);
            return RedirectToAction("OrderList");

        }

        [HttpGet]
        public IActionResult UpdateOrder(int id)
        {
            var order = _orderService.GetById(id);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrder(Order order)
        {
            order.OrderDate = DateTime.Now;

            _orderService.Update(order);
            return RedirectToAction("OrderList");
        }

        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            _orderService.Delete(id);
            return RedirectToAction("OrderList");
        }
    }
}

