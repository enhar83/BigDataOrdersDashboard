using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class PaymentForecastResultDto
    {
        public string PaymentMethod { get; set; }
        public string Month { get; set; }
        public int PredictedCount { get; set; }
        public int LastYearsCount { get; set; } 
    } 
}

/*
    * PaymentForecastResultDto: ham verilerin (float) kullanıcıya gösterilmeden önce düzenlenmiş halidir. float yerine int, ay ismi olarak string 
 */

/*
    * Controller, ResultDtodan gelen ham sayıları alır ve kullanıcıya gösterilecek şık bir nesneye dönüştürür.
    * Amacı: Ham sayıları insanların anlayacağı hale getirmek. (Ay ismi eklemek, küsuratı atmak)
 */