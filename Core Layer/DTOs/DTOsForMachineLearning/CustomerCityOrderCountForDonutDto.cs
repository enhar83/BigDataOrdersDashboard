using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerCityOrderCountForDonutDto
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public int OrderCount { get; set; }
        public double Percentage { get; set; }
    }
}
