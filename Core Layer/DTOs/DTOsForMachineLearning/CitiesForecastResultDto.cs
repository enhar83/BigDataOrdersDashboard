using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CitiesForecastResultDto
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string Month { get; set; }
        public int PredictedCount { get; set; }
        public int CountFor2024 { get; set; }
        public int CountFor2025 { get; set; }
        public double ChangeRate { get; set; }
    }
}
