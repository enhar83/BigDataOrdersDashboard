using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class PaymentForecastPredictionDTO
    {
        public float[] ForecastedValues { get; set; }
    }
}

/*
   * PaymentForecastPredictionDto: modelin sınav sonucu veya gelecek raporudur.
       -> float[] ForecastedValues: ML.NET toplu tahmin üretir. Ocak, Şubat ve Mart tahminlerini bir dizi olarak teslim eder.
 */

/*
   * PredictionManager içindeki Fit() ve Predict() işlemleri, ML.NET bu DTO'yu fırlatır.
   * Amacı: Gelecek tahminlerini ham bir şekilde teslim etmek.
       - ForecastedValues (float[]): [133.1, 164.4, 161.0]
   
   * Neden Bu DTO?: ML.NET tahminleri tek tek değil, bir dizi olarak verir. Burada model şunu diyor Geçmişteki %10 artışı gördüm; Nisan'da 133, Mayıs'ta 146 bekliyorum.
 */
