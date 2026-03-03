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
            return View();
        }
    }
}
