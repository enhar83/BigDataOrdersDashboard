using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisCustomerInsightsWithGeminiDto
    {
        public string CustomerFullName { get; set; }
        public List<SentimentAnalysisOrderDetailsForCustomerInsightsWithGeminiDto> LastOrders { get; set; }
        public string AiAnalysis  { get; set; }
    }
}

//bu sınıf ekrana gidecek ana veri paketi.
//içerisindeki diğer dto ise her siparişin detayını taşımak için kullanılmaktadır.
