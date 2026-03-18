using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class SentimentAnalysisMainCoverTableDto
    {
        public string? CustomerFullName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerImage { get; set; }
        public string? CustomerDescription { get; set; }
    }
}
