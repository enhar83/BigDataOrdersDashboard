using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerLoyaltyScoreDto
    {
        public string CustomerName { get; set; }
        public int OrderCount { get; set; }
        public double TotalSpend { get; set; }
        public DateTime? LastOrderDate { get; set; } //müşterinin hiç siparişi olmama ihtimalinden dolayı
        public double LoyaltyScore { get; set; }
    }
}
