using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs
{
    public class KpiCartsDto
    {
        public int TodayOrdersCount { get; set; }
        public int YesterdayOrdersCount { get; set; }
        public double TodayOrdersPrice { get; set; }
        public double YesterdayOrdersPrice { get; set; }
        public double TodayOrdersAveragePrice { get; set; }
        public double YesterdayOrdersAveragePrice { get; set; }
        public double OrdersCountPercentageChange { get; set; }
        public double OrdersPricePercentageChange { get; set; }
        public double OrdersAveragePricePercentageChange { get; set; }
    }
}
