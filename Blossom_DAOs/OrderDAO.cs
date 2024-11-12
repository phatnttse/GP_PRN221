using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blossom_DAOs
{
    public class OrderDAO
    {
        private readonly ApplicationDbContext _context;

        public OrderDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> GetOrders()
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails) // Eager load OrderDetails
                .ThenInclude(od => od.Flower) // Optional: Include related Flower if needed
                .ThenInclude(od => od.Seller) // Optional: Include Seller if needed
                .ToList();

            return orders;
        }

        public Order GetOrder(string id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Flower)
                .ThenInclude(od => od.Seller)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return order;
        }

        public bool AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges(); // Consider wrapping this in a try-catch or transaction for better error handling
            return true;
        }

        public bool UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOrder(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}
