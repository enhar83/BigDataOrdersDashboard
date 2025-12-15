using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs
{
    public class CountryReportDto
    {
        public string Country { get; set; }
        public int Total2023 { get; set; }
        public int Total2024 { get; set; }
        public decimal ChangeRate { get; set; }
    }
}
