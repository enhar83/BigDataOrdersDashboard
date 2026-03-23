using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class ToxicBertCheckResultDto
    {
        public bool IsToxic { get; set; }       
        public string TopLabel { get; set; } = "";
        public float ConfidenceScore { get; set; }
    }
}
