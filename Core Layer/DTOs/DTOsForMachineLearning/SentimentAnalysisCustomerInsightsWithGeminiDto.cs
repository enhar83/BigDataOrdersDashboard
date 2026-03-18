using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisCustomerInsightsWithGeminiDto
    {
        public int TotalOrderCount { get; set; }
        public double TotalSpent { get; set; }
        public string MostPurchasedCategory { get; set; }
        public string FrequentOrderTime { get; set; }
        public string AIGeneratedAnalysis { get; set; } //aidan gelen analiz metni
        public string SuggestedProducts { get; set; } //aidan gelen önerilmesi gereken ürünler
    }
}
