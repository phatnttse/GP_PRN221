using Blossom_BusinessObjects;
using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        List<OrderDetail> GetOrderDetails(string orderId);
        OrderDetail GetOrderDetail(string id);
        bool AddOrderDetail(OrderDetail orderDetail);
        bool UpdateOrderDetail(OrderDetail orderDetail);
        bool DeleteOrderDetail(string id);

        List<OrderDetail> GetOrderDetailsById(string username);
        List<OrderDetail> GetOrderDetailsBySellerId(string username);

        bool UpdateOrderStatusByOrderDetailId(string orderDetailId, int status);

        decimal GetTotalRevenueAsync(DateTime startDate, DateTime endDate, string userId);
        int GetTotalOrdersCountAsync(DateTime startDate, DateTime endDate, string userId);
        int GetTotalFlowerViewsAsync(DateTime startDate, DateTime endDate, string userId);

        List<DateTime> GetDateRangeList(DateTime startDate, DateTime endDate);

        List<RevenueByDate> GetDailyRevenueAsync(DateTime startDate, DateTime endDate, string userId);
    }
}
