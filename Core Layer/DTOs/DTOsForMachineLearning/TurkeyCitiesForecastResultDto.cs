using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class TurkeyCitiesForecastResultDto
    {
        public string CityName { get; set; }
        public string Month  { get; set; }
        public int PredictedCount { get; set; }
        public int LastYearsCount { get; set; }
        public double ChangeRate { get; set; }
    }
}
