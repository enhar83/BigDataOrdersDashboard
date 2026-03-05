using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class GermanyCitiesForecastDataDto
    {
        public string CityName { get; set; }
        public float MonthIndex { get; set; } //aslında 2 yıllık veriyle çalışılacak. Ancak ML.NET en az 3 birim istediğinden dolayı az bazlı aldık ve 24 birim oldu. Aslında aynı şeye denk geliyor.
        public float OrderCount { get; set; }
    }
}
