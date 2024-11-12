using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace Blossom_DAOs
{
    public class OrderDetailDAO
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all order details for a specific order
        public Task<List<OrderDetail>> GetOrderDetails(string orderId)
        {
            var orderDetails = _context.OrderDetails
                .Include(od => od.Order)      // Include the associated order
                .Include(od => od.Flower)     // Include the associated flower (if needed)
                .Include(od => od.Seller)     // Include the associated seller (if needed)
                .Where(od => od.OrderId == orderId)
                .ToList();

            return Task.FromResult(orderDetails);
        }

        // Get a specific order detail by ID
        public Task<OrderDetail> GetOrderDetail(string id)
        {
            var orderDetail = _context.OrderDetails
                .Include(od => od.Order)      // Include the associated order
                .Include(od => od.Flower)     // Include the associated flower (if needed)
                .Include(od => od.Seller)     // Include the associated seller (if needed)
                .FirstOrDefault(od => od.Id == id); // Use FirstOrDefault instead of async method

            if (orderDetail == null)
            {
                throw new Exception("OrderDetail not found");
            }

            return Task.FromResult(orderDetail);
        }

        // Add a new order detail
        public Task<bool> AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges(); // Consider wrapping this in a try-catch or transaction for better error handling
            return Task.FromResult(true);
        }

        // Update an existing order detail
        public Task<bool> UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        // Delete an order detail by its ID
        public Task<bool> DeleteOrderDetail(string id)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.Id == id);
            if (orderDetail == null)
            {
                throw new Exception("OrderDetail not found");
            }

            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<List<OrderDetail>> GetOrderDetailsByUserName(string accountId)
        {
            var orderDetails = _context.OrderDetails
                .Include(od => od.Order)      
                .Include(od => od.Flower)     
                .Include(od => od.Seller)    
                .Where(od => od.Order.UserId == accountId)
                .ToList();

            return Task.FromResult(orderDetails);
        }
    }
}
