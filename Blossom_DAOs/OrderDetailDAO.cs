using Blossom_BusinessObjects;
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

        public Task<List<OrderDetail>> GetOrderDetailsBySellerId(string accountId)
        {
            var orderDetails = _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Flower)
                .Include(od => od.Seller)
                .Where(od => od.SellerId == accountId)
                .ToList();

            return Task.FromResult(orderDetails);
        }

        public Task<bool> UpdateOrderStatusByOrderDetailId(string orderDetailId, int status)
        {
            var orderDetail = _context.OrderDetails
                .FirstOrDefault(od => od.Id == orderDetailId);

            if (orderDetail != null)
            {
                orderDetail.Status += status;

                _context.SaveChanges();

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

         public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate, string userId)
        {
            var totalRevenue = await _context.Set<OrderDetail>()
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted && o.Flower.SellerId.Equals(userId))
                .SumAsync(o => o.Order.TotalPrice);

            return totalRevenue;
        }

        public async Task<int> GetTotalOrdersCountAsync(DateTime startDate, DateTime endDate, string userId)
        {
            var totalOrdersCount = await _context.Set<OrderDetail>()
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted && o.Flower.SellerId.Equals(userId))
                .CountAsync();

            return totalOrdersCount;
        }

        public async Task<int> GetTotalFlowerViewsAsync(DateTime startDate, DateTime endDate, string userId)
        {
            var totalFlowerViews = await _context.Flowers
                .Where(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate && f.SellerId.Equals(userId))
                .SumAsync(f => f.Views);

            return totalFlowerViews;
        }
        public List<DateTime> GetDateRangeList(DateTime startDate, DateTime endDate)
        {
            var dateList = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dateList.Add(date);
            }

            return dateList;
        }

        public async Task<List<RevenueByDate>> GetDailyRevenueAsync(DateTime startDate, DateTime endDate, string userId)
        {
            var allDates = GetDateRangeList(startDate, endDate);

            var dailyRevenue = await _context.Set<OrderDetail>()
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.Flower.SellerId.Equals(userId) && !o.IsDeleted)
                .GroupBy(o => o.CreatedAt.Date) 
                .Select(g => new RevenueByDate
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(o => o.Order.TotalPrice)
                })
                .ToListAsync();

            var result = allDates.Select(date =>
            {
                var revenue = dailyRevenue.FirstOrDefault(r => r.Date.Date == date.Date);
                return new RevenueByDate
                {
                    Date = date,
                    TotalRevenue = revenue != null ? revenue.TotalRevenue : 0 
                };
            }).ToList();

            return result;
        }

    }
}
