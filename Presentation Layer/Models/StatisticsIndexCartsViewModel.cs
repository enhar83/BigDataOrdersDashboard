namespace Presentation_Layer.Models
{
    public class StatisticsIndexCartsViewModel
    {
        public int CategoryCount { get; set; }
        public int ProductCount { get; set; }
        public int CustomerCount { get; set; }
        public int OrderCount { get; set; }
        public int CompletedOrderCount { get; set; }
        public int CancelledOrderCount { get; set; }
        public int DistinctCountryCount { get; set; }
        public int DistinctCityCount { get; set; }
        public string MostOrderingCountry { get; set; }
        public string MostOrderingCustomer { get; set; }
        public int ThisYearOrders { get; set; }
        public string MostProducedProductCountry { get; set; }
    }
}
