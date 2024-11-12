using Blossom_BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();

        Order GetOrder(string id);

        bool AddOrder(Order order);

        bool UpdateOrder(Order order);

        bool DeleteOrder(string id);

        List<Order> GetAllOrdersById(string username);
    }
}
