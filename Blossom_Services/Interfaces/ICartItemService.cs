using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface ICartItemService
    {
        // Get all cart items for a specific user
        Task<IEnumerable<CartItem>> GetAllByUserAsync(Account user);

        Task<IEnumerable<CartItem>> GetAllCartItemUserIdAsync(string user);

        Task AddFlowerListingToCart(string user, string flowerId, int quantity);

        Task ReduceFlowerListingQuantityInCart(string user, string flowerId, int quantity);

        // Find a cart item by user and flower ID
        Task<CartItem> GetByUserAndFlowerAsync(string user, string flowerID);

        // Delete all cart items for a specific user
        Task DeleteAllByUserAsync(Account user);


        // Count cart items for a specific flower user within a date range
        Task<int> CountCartItemsByFlowerUserAndDateRangeAsync(Account seller, DateTime startDate, DateTime endDate);

        Task<bool> UpdateCartItem(CartItem cartItem);

        Task DeleteCartItem(string user, string flowerId);

    }
}
