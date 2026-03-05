using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Presentation_Layer.Models.MachineLearningViewModels
{
    public class GermanyCitiesForecastViewModel
    {
        public string SelectedGermanyCity { get; set; }
        public List<GermanyCitiesForecastResultDto> ForecastResults { get; set; }
    }
}
