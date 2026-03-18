using Business_Layer.MachineLearning.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.ViewComponents.SentimentAnalysisViewComponents
{
    public class _SentimentAnalysisSubHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
