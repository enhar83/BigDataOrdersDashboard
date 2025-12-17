using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs
{
    public class MainChartDto
    {
        public decimal TodayOrdersPrice { get; set; }
        public decimal ThisMonthOrdersPrice { get; set; }
        public decimal LastSixMonthsOrdersPrice { get; set; }
    }
}
