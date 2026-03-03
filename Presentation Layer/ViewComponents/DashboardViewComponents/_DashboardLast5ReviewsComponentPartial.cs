using Business_Layer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.DashboardViewComponents
{
    public class _DashboardLast5ReviewsComponentPartial : ViewComponent
    {
        private readonly IReviewService _reviewService;

        public _DashboardLast5ReviewsComponentPartial(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IViewComponentResult Invoke()
        {
            var last5Reviews = _reviewService.GetLast5Reviews();
            return View(last5Reviews);
        }
    }
}
