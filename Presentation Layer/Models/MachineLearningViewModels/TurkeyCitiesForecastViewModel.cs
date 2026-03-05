using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Presentation_Layer.Models.MachineLearningViewModels
{
    public class TurkeyCitiesForecastViewModel
    {
        public List<string> CityNames { get; set; }
        public string SelectedTurkeyCity { get; set; }
        public List<TurkeyCitiesForecastResultDto> ForecastResults { get; set; }
    }
}
