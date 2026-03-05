using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerAnalyticsMainStatisticsDto
    {
        public int TotalCustomerCount { get; set; }
        public int AverageOrderCountPerPerson { get; set; }
        public double AverageSpending { get; set; }
        public string MostActiveCity { get; set; }
    }
}
