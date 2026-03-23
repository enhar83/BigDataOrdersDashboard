using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IActionResult MessageList(int page = 1)
        {
            int pageSize = 12;

            var (values, totalCount) = _messageService.GetMessagesWithPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(values);
        }

        [HttpGet]
        public IActionResult AddMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMessage(Message message)
        {
            message.CreatedDate = DateTime.Now;
            _messageService.Add(message);
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public IActionResult UpdateMessage(int id)
        {
            var messageToUpdate = _messageService.GetFirstOrDefault(id);
            if (messageToUpdate == null) return NotFound();
            return View(messageToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMessage(Message message)
        {
            _messageService.Update(message);
            return RedirectToAction("MessageList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMessage(int id)
        {
            _messageService.Delete(id);
            return RedirectToAction("MessageList");
        }
    }
}
