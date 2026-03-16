using Core_Layer.DTOs.DTOsForMachineLearning;

namespace Presentation_Layer.Models.MachineLearningViewModels
{
    public class CustomerLoyaltyWithMlViewModel
    {
        public List<CustomerLoyaltyScoreResultMLDto> LoyaltyScores { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }
    }
}
