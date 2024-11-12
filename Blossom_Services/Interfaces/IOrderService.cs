using Blossom_BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();

        Order GetOrderById(string id);

        bool AddOrder(Order order);

        bool UpdateOrder(Order order);

        bool DeleteOrder(string id);

        List<Order> GetOrdersById(string user);
    }
}
