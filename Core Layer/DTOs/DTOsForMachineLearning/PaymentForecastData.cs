using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class PaymentForecastData
    {
        public string PaymentMethod { get; set; }
        public float MonthIndex { get; set; }
        public float OrderCount { get; set; }
    }
}

/*
Giriş verisi olarak kullanılacaktır.
Bu sınıf ML.NET modeline geçmişte ne olduğunu anlatmak için kullanılır.
    - PaymentMethod: Modelin farklı ödeme yöntemleri arasındaki davranış farklarını anlamasını sağlar.
    - MonthIndex: Zaman serisi analizlerinde zamanın akışını temsil eder. ML.NET 1. aydan 24. aya kadar olan dizilimi takip ederek artış veya azalış trendini bu index üzerinden yakalar.
    - OrderCount: Label denilen hedef değeridir. Model, PaymentMethod ve MonthIndex verildiğinde bu rakama nasıl ulaşıldığını öğrenmeye çalışır.
*/
