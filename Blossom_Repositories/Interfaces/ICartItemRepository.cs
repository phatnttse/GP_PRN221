using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        // Get all cart items for a specific user
        Task<IEnumerable<CartItem>> GetAllByUserAsync(Account user);

        Task<IEnumerable<CartItem>> GetAllCartItemUserIdAsync(string user);

        Task<bool> AddFlowerListingToCart(string user, CartItem cartItem);

        // Find a cart item by user and flower ID
        Task<CartItem> GetByUserAndFlowerAsync(string user, string flowerID);

        // Delete all cart items for a specific user
        Task DeleteAllByUserAsync(Account user);

        Task DeleteCartItem(CartItem cartItem);


        // Count cart items for a specific flower user within a date range
       Task<int> CountCartItemsByFlowerUserAndDateRangeAsync(Account seller, DateTime startDate, DateTime endDate);

        Task<bool> UpdateCartItem(CartItem cartItem);

    }
}
