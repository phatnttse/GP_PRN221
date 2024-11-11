using Blossom_BusinessObjects.Entities;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetOrders();
        }

        public Order GetOrderById(string id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            return order;
        }
        public bool AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return _orderRepository.AddOrder(order);
        }

        public bool UpdateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return _orderRepository.UpdateOrder(order);
        }
        public bool DeleteOrder(string id)
        {
            return _orderRepository.DeleteOrder(id);
        }
    }
}
