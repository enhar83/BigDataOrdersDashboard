using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisReviewDetailsForReviewInsightsWithGeminiDto
    {
        public string ReviewText { get; set; }
        public string Sentiment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
