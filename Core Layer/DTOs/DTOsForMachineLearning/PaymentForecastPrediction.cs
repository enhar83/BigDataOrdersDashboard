using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class PaymentForecastPrediction
    {
        public float[] ForecastedValues { get; set; }
    }
}

/*
Çıkış verisidir. Modelin eğitim bittikten sonra verdiği gelecek raporudur.
   - ForecastedValues (float[]): Forecasting senaryolarında model genellikle tek bir değer yerine bir array döner. 2026'nın ilk 3 ayı istendiği için, bu dizinin içinde sırasıyla [Ocak_Tahmini, Subat_Tahmini, Mart_Tahmini] değerleri yer alacaktır.
  
   - Neden float[]?: ML.NET'in zaman serisi algoritmaları belirli bir zaman penceresi için toplu tahmin üretme eğilimindedir.
        * Horizon Mantığı: ML.NET zaman serisi algoritmaları, bana bir sonraki adımı söyle demek yerine bana önümüzdeki X birimlik zaman dilimini komple tahmin et mantığıyla çalışır. Ocak, Şubat, Mart tahmin edildiğinden dolayı horizon burada 3tür.
        * Tahminlerin Birbirine Bağlılığı: Bu modelde Ocak, Şubatı ; Şubat da Martı etkiler. Yani Ocak hesaplandıktan sonra, Şubat hesaplanırken Ocak verisi de kullanılır.
        * ForecastedValues[0] --> 2026 Ocak Tahmini
        * ForecastedValues[1] --> 2026 Şubat Tahmini
        * ForecastedValues[2] --> 2026 Mart Tahmini
        
        * ML.NET içerisinde zaman serisi tahminleme için kullanılan TimeSeriesPredictionEngine yapısı, çıktı sınıfında (PaymentForecastPrediction) tahmin edilen değerlerin bir dizi olmasını zorunlu kılar. Eğer bunu tek bir float yaparsan, algoritma 3 aylık tahmini nereye yazacağını bilemez ve hata verir. 
*/
