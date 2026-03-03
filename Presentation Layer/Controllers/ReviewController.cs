using Business_Layer.Abstract;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IActionResult ReviewList(int page = 1)
        {
            int pageSize = 12;
            var (reviews, totalCount) = _reviewService.GetReviewsWithPaging(page, pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(reviews);
        }

        [HttpGet]
        public IActionResult AddReview()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(Review review)
        {
            review.ReviewDate = DateTime.Now;

            _reviewService.Add(review);
            return RedirectToAction("ReviewList");
        }

        [HttpGet]
        public IActionResult UpdateReview(int id)
        {
            var reviewToUpdate = _reviewService.GetFirstOrDefault(id);
            reviewToUpdate.ReviewDate = DateTime.Now;

            if (reviewToUpdate == null) return NotFound();
            return View(reviewToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateReview(Review review)
        {
            _reviewService.Update(review);
            return RedirectToAction("ReviewList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(int id)
        {
            _reviewService.Delete(id);
            return RedirectToAction("ReviewList");
        }
    }
}
