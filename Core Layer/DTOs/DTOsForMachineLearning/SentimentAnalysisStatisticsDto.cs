using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisStatisticsDto
    {
        public int CustomerTotalOrderCount { get; set; }
        public int CustomerCompletedOrderCount { get; set; }
        public int CustomerCancelledOrderCount { get; set; }
        public string CustomerCountryCityInformation { get; set; }
        public int CustomerReviewCount { get; set; }
        public double CustomerTotalPurchase { get; set; }
    }
}
