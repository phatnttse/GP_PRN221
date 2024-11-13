using Blossom_BusinessObjects.Entities;
using Blossom_Repositories;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUserIdAssessor _userIdAssessor;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IUserIdAssessor userIdAssessor)
        {
            _orderDetailRepository = orderDetailRepository;
            _userIdAssessor = userIdAssessor;
        }

        public List<OrderDetail> GetOrderDetails(string orderId)
        {
            return _orderDetailRepository.GetOrderDetails(orderId);
        }

        public OrderDetail GetOrderDetail(string id)
        {
            return _orderDetailRepository.GetOrderDetail(id);
        }

        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailRepository.AddOrderDetail(orderDetail);
        }

        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailRepository.UpdateOrderDetail(orderDetail);
        }

        public bool DeleteOrderDetail(string id)
        {
            return _orderDetailRepository.DeleteOrderDetail(id);
        }

        public List<OrderDetail> GetOrderDetailsById(string username)
        {
            var existingUser = _userIdAssessor.GetCurrentUserId();
            if (existingUser != null) { 
                var orderDetail =_orderDetailRepository.GetOrderDetailsById(username);
                return orderDetail.ToList() ?? new List<OrderDetail>();
            }
            return new List<OrderDetail>();
        }

        public List<OrderDetail> GetOrderDetailsBySellerId(string username)
        {
            var existingUser = _userIdAssessor.GetCurrentUserId();
            if (existingUser != null)
            {
                var orderDetail = _orderDetailRepository.GetOrderDetailsBySellerId(username);
                return orderDetail.ToList() ?? new List<OrderDetail>();
            }
            return new List<OrderDetail>();
        }

        public bool UpdateOrderStatusByOrderDetailId(string orderDetailId, int status) => _orderDetailRepository.UpdateOrderStatusByOrderDetailId(orderDetailId, status);

        public decimal GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var existingUser = _userIdAssessor?.GetCurrentUserId();
            if (existingUser != null)
            {
                return _orderDetailRepository.GetTotalRevenueAsync(startDate, endDate, existingUser);
            }
            return 0;
        }

        public int GetTotalOrdersCountAsync(DateTime startDate, DateTime endDate)
        {
            var existingUser = _userIdAssessor?.GetCurrentUserId();
            if (existingUser != null)
            {
                return _orderDetailRepository.GetTotalOrdersCountAsync(startDate, endDate, existingUser);
            }
            return 0;
        }

        public int GetTotalFlowerViewsAsync(DateTime startDate, DateTime endDate)
        {
            var existingUser = _userIdAssessor?.GetCurrentUserId();
            if (existingUser != null)
            {
                return _orderDetailRepository.GetTotalFlowerViewsAsync(startDate, endDate, existingUser);
            }
            return 0;
        }
    }
}
