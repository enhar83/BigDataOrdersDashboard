using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs
{
    public class CountryOrderCountForDonutDto
    {
        public string CountryName { get; set; }
        public int OrderCount { get; set; }
        public double Percentage { get; set; }
    }
}
