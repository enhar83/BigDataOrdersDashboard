using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForDashboard
{
    public class OrderStatusChartDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public double Rate { get; set; }
    }
}
