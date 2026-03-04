using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class PaymentForecastDataDTO
    {
        public string PaymentMethod { get; set; }
        public float MonthIndex { get; set; }
        public float OrderCount { get; set; }
    }
}

/*
    * PaymentForecastDataDto: modelin öğrenci dosyasıdır. Modelin geçmişte ne olduğunu anlaması için kullanılır.
         -> MonthIndex: zamanın akışını (1,2,3...) temsil eder. Algoritma tarihleri değil, sayısal dizilimleri anlar.
         -> OrderCount: hedef değerdir. Model bu sayının zamanla nasıl değiştiğini öğrenir.
 */

/*
    * Dbde 2025 yılı için PayPal verileri şöyle olsun:
       - Ocak 2025: 100
       - Şubat 2025: 110
       - Mart 2025: 121 (sürekli %10 artan bir pattern var)
 */

/*
     * PredictionManager çalışmaya başladığında, SQL'den gelen verileri bu DTO'nun içerisine koyar.
     * Amacı: Algoritmaya geçmişi ML.NET'in anlayacağı dilden vermek.
     * Neden Bu DTO?: Algoritma Ocak kelimesini bilmez 1. ayda 100, 2. ayda 110 satmış diyerek aradaki %10 artış matematiğini çözer.
 */