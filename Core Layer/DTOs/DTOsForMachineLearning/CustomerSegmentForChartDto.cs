using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerSegmentForChartDto
    {
        public string SegmentName { get; set; }
        public int Count { get; set; }
        public double AverageSegmentSpend { get; set; }
    }
}
