using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForDashboard
{
    public class MainChartDto
    {
        public decimal CompletedOrdersPrice { get; set; }
        public decimal CancelledOrdersPrice { get; set; }
        public decimal ShippingOrdersPrice { get; set; }
    }
}
