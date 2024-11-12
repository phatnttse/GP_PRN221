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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
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
    }
}
