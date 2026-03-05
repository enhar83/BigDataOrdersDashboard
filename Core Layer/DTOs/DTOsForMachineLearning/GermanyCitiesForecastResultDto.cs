using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class GermanyCitiesForecastResultDto
    {
        public string CityName { get; set; }
        public string Month { get; set; }
        public int PredictedCount { get; set; }
        public int LastYearsCount { get; set; }
    }
}
