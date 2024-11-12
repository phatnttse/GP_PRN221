using Blossom_BusinessObjects.Entities;
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
    }
}
