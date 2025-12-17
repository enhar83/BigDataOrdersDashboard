using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.DTOs;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order GetById(int id);
        Order GetFirstOrDefault(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        (List<Order> orders, int totalCount) GetOrdersWithPaging(int pageNumber, int pageSize);
        int CountOrders();
        int CountCompletedOrders();
        int CountCancelledOrders();
        int GetThisYearOrders();
        decimal GetTotalRevenue();
        decimal GetAverageRevenue();
        string GetMostOrderedProduct();
        string GetLeastOrderedProduct();
        string GetMostOrderedCustomer();
        string GetMostOrderedCategory();
        string GetMostOrderedCity();
        string GetMostOrderedCountry();
        string GetMostOrderedProductThisMonth();
        string GetMostCancelledProduct();
        string GetMostOrderedPayment();
        string GetMostCompletedProductName();
        List<TodayOrdersDto> GetLast10OrdersToday();
        List<CountryReportDto> GetCountryReportForMap();
        List<OrderStatusChartDto> GetOrderStatusChartData();
        KpiCartsDto CompareTodayAndYesterdayOrdersForKpiCarts();
    }

}
