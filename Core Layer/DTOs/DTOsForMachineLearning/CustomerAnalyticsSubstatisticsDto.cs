using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerAnalyticsSubstatisticsDto
    {
        public string MostActiveCustomerName { get; set; }
        public int MostActiveCustomerOrderCount { get; set; }
        public string LeastActiveCustomerName { get; set; }
        public int LeastActiveCustomerOrderCount { get; set; }
        public string RisingStarCustomerName { get; set; }
        public int RisingStarCustomerOrderCount { get; set; }
        public string FallingStarCustomerName { get; set; }
        public int FallingStarCustomerOrderCount { get; set; }
    }
}
