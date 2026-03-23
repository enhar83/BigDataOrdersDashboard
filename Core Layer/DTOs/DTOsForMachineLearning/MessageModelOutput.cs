using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class MessageModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; } = "";
        public float[] Score { get; set; }
    }
}
