using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Business_Layer.MachineLearning.Abstract
{
    public interface ICustomerAnalyticsService
    {
        CustomerAnalyticsMainStatisticsDto GetCustomerAnalyticsMainStatistics();
        List<CustomerSegmentForChartDto> GetCustomerSegmentForChart();
    }
}
