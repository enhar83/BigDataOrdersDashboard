using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisReviewInsightsWithGeminiDto
    {
        public string CustomerFullName { get; set; }
        public string AiAnalysis { get; set; }
        public List<SentimentAnalysisReviewDetailsForReviewInsightsWithGeminiDto> LastReviews { get; set; }
    }
}
