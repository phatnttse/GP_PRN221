using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Blossom_Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;

        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public List<Order> GetOrders()
        {
            return _orderDAO.GetOrders();
        }

        public Order GetOrder(string id)
        {
            return _orderDAO.GetOrder(id);
        }

        public bool AddOrder(Order order)
        {
            return _orderDAO.AddOrder(order);
        }

        public bool UpdateOrder(Order order)
        {
            return _orderDAO.UpdateOrder(order);
        }

        public bool DeleteOrder(string id)
        {
            return _orderDAO.DeleteOrder(id);
        }
    }
}
