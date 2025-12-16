using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.Helpers
{
    public class GetColorForCharts
    {
        public static string GetColor(string status)
        {
            return status switch
            {
                "Tamamlandı" => "#66BB6A",
                "Hazırlanıyor" => "#FFB300",
                "Kargoda" => "#2196F3",
                "İptal Edildi" => "#EF5350",
                "Beklemede" => "#9C27B0",
                _ => "#BDBDBD"
            };
        }
    }
}
