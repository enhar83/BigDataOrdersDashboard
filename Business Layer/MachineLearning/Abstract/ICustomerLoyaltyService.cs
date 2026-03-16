using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Business_Layer.MachineLearning.Abstract
{
    public interface ICustomerLoyaltyService
    {
        List<CustomerLoyaltyScoreDto> GetCustomerLoyaltyScores(string cityName);
        List<CustomerLoyaltyScoreResultMLDto>  GetCustomerLoyaltyScoresWithML(string cityName);
        List<string> GetDistictCountryNames();
        List<string> GetDistictCityNames(string countryName);
    }
}
