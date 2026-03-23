using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class MessageModelInputDto
    {
        public string MessageSubject { get; set; } = "";
        public string MessageText { get; set; } = "";
        public string SentimentLabel { get; set; } = "";
    }
}
