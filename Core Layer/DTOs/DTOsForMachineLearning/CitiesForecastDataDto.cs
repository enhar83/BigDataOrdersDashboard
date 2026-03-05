using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CitiesForecastDataDto
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public float MonthIndex { get; set; }
        public float OrderCount { get; set; }
    }
}
