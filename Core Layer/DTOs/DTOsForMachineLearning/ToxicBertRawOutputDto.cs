using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class ToxicBertRawOutputDto
    {
        public string Label { get; set; } = "";
        public float Score { get; set; }
    }
}
