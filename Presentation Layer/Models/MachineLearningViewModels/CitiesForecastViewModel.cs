using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Presentation_Layer.Models.MachineLearningViewModels
{
    public class CitiesForecastViewModel
    {
        public string SelectedCountry { get; set; }
        public List<string> CountryNames { get; set; }
        public string SelectedCity { get; set; }
        public List<string> CityNames { get; set; }
        public List<CitiesForecastResultDto> ForecastResults { get; set; }
    }
}
