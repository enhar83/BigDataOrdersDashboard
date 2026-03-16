using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Core_Layer.DTOs.DTOsForMachineLearning
{
    public class CustomerLoyaltyScorePredictionMLDto
    {
        [ColumnName("Score")] //ML.Net modelden çıkan "Score" verisini nereye koyacağını bilemez. LoyaltyScore prop'u ile eşleştirerek hata alınması önlenir.
        public float LoyaltyScore { get; set; }
    }
}

//ColumnName kullanmadan direkt olarak prop adı Score'da yapılabilirdi ama açık ve net bir isim konulması önerilir.

//Label: Tahmin edilecek gerçek değer.
//Features: Tahmin için kullanılan girdiler. (R,F,M)
//Score: Modelin yaptığı tahminin nihai çıktısıdır. 

//bu dto içerisinde algoritmadan çıkan sonuç yakalanır. Modelden gelen veri LoyaltyScore içerisine hapsolur.