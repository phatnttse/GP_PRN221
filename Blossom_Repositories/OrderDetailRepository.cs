using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Blossom_Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderDetailDAO _orderDetailDAO;

        public OrderDetailRepository(OrderDetailDAO orderDetailDAO)
        {
            _orderDetailDAO = orderDetailDAO;
        }

        public List<OrderDetail> GetOrderDetails(string orderId)
        {
            return _orderDetailDAO.GetOrderDetails(orderId).Result;
        }

        public OrderDetail GetOrderDetail(string id)
        {
            return _orderDetailDAO.GetOrderDetail(id).Result;
        }

        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailDAO.AddOrderDetail(orderDetail).Result;
        }

        // Update an existing order detail
        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailDAO.UpdateOrderDetail(orderDetail).Result;
        }

        public bool DeleteOrderDetail(string id)
        {
            return _orderDetailDAO.DeleteOrderDetail(id).Result;
        }

        public List<OrderDetail> GetOrderDetailsById(string username) => _orderDetailDAO.GetOrderDetailsByUserName(username).Result;

        public List<OrderDetail> GetOrderDetailsBySellerId(string username) => _orderDetailDAO.GetOrderDetailsBySellerId(username).Result;

        public bool UpdateOrderStatusByOrderDetailId(string orderDetailId, int status) => _orderDetailDAO.UpdateOrderStatusByOrderDetailId(orderDetailId, status).Result;
    }
}
