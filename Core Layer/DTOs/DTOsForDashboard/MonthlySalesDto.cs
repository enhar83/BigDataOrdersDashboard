using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForDashboard
{
    public class MonthlySalesDto
    {
        public string MonthName { get; set; }
        public int CompletedCount { get; set; }
        public decimal CompletedAmount { get; set; }
        public int CancelledCount { get; set; }
        public decimal CancelledAmount { get; set; }
        public int ShippedCount { get; set; }
        public decimal ShippedAmount { get; set; }
    }
}
