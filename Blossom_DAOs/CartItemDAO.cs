using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_DAOs
{
    public class CartItemDAO 
    {
        private ApplicationDbContext _context;
        public CartItemDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all cart items for a specific user
        public async Task<IEnumerable<CartItem>> GetAllByUserAsync(Account user)
        {
            return await _context.CartItems.Where(ci => ci.UserId == user.Id).ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemUserIdAsync(string user)
        {
            var cartItems = await _context.CartItems.Where(ci => ci.UserId.Equals(user)).Include(li => li.Flower).ToListAsync();
            return cartItems;
        }

        public async Task<bool> AddFlowerListingToCart(string user, CartItem cartItem)
        {
            bool result = false;
            try
            {
                await _context.CartItems.AddAsync(cartItem);
                _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        // Find a cart item by user and flower ID
        public async Task<CartItem> GetByUserAndFlowerAsync(string user, string flowerID)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId.Equals(user) && ci.Flower.Id.Equals(flowerID));
        }

        // Delete all cart items for a specific user
        public async Task DeleteAllByUserAsync(Account user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var items = await GetAllByUserAsync(user);

            if (items != null && items.Any())
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteCartItem(CartItem cartItem)
        {
            bool result = false;
            try
            {
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        // Count cart items for a specific flower user within a date range
        public async Task<int> CountCartItemsByFlowerUserAndDateRangeAsync(Account seller, DateTime startDate, DateTime endDate)
        {
            if (seller == null)
            {
                throw new ArgumentNullException(nameof(seller), "Seller cannot be null.");
            }

            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be earlier than or equal to end date.");
            }

            return await _context.CartItems
                .CountAsync(ci => ci.Flower.SellerId == seller.Id &&
                                  ci.CreatedAt >= startDate &&
                                  ci.CreatedAt <= endDate);
        }

        public async Task<bool> UpdateCartItem(CartItem cartItem)
        {
            bool result = false;
            try
            {
                _context.CartItems.Update(cartItem);
                _context.SaveChanges();
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }


    }
}
