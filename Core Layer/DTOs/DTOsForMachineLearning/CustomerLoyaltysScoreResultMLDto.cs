using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerLoyaltysScoreResultMLDto
    {
        public string CustomerName { get; set; }
        public double Recency { get; set; }
        public double Frequency { get; set; }
        public double Monetary { get; set; }
        public double ActualLoyaltyScore { get; set; }
        public double PredictedLoyaltyScore { get; set; }
    }
}
