using Blossom_BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface IOrderDetailService
    {
        List<OrderDetail> GetOrderDetails(string orderId);
        OrderDetail GetOrderDetail(string id);
        bool AddOrderDetail(OrderDetail orderDetail);
        bool UpdateOrderDetail(OrderDetail orderDetail);
        bool DeleteOrderDetail(string id);
        List<OrderDetail> GetOrderDetailsById(string username);

        List<OrderDetail> GetOrderDetailsBySellerId(string username);

        bool UpdateOrderStatusByOrderDetailId(string orderDetailId, int status);

        decimal GetTotalRevenueAsync(DateTime startDate, DateTime endDate);

        int GetTotalOrdersCountAsync(DateTime startDate, DateTime endDate);

        int GetTotalFlowerViewsAsync(DateTime startDate, DateTime endDate);
    }
}
