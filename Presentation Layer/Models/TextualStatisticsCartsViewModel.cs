namespace Presentation_Layer.Models
{
    public class TextualStatisticsCartsViewModel
    {
        public string? MostExpensiveProduct { get; set; }
        public string? CustomerWithMostOrders { get; set; }
        public string? CategoryWithMostOrders { get; set; }
        public string? CityWithMostOrders { get; set; }
        public string? CountryWithMostOrders { get; set; }
        public string? LeastStockedProduct { get; set; }
        public string? MostOrderedProductThisMonth { get; set; }
        public string? MostCancelledProduct { get; set; }
        public string? LastAddedCustomer { get; set; }
        public string? FirstAddedCustomer { get; set; }
        public string? MostOrderedPayment { get; set; }
    }
}
