using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerLoyaltyScoreDataMLDto
    {
        public string CustomerName { get; set; }
        public float Recency { get; set; }
        public float Frequency { get; set; }
        public float Monetary { get; set; }
        public float LoyaltyScore { get; set; }
    }
}

//modelin eğitilmesi için gerekli olan ham maddedir. ML.NET'e müşterilerin verileri bunlar denilen sınıftır.
