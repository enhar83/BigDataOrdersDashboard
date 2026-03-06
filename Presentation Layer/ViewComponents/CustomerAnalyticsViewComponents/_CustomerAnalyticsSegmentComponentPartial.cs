using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsSegmentComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
