using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private CartItemDAO _cartItemDAO;
        public CartItemRepository(CartItemDAO cartItemDAO) { 
            _cartItemDAO = cartItemDAO; 
        }

        public Task<bool> AddFlowerListingToCart(string user, CartItem cartItem) => _cartItemDAO.AddFlowerListingToCart(user, cartItem);   

        public Task<int> CountCartItemsByFlowerUserAndDateRangeAsync(Account seller, DateTime startDate, DateTime endDate)=> _cartItemDAO.CountCartItemsByFlowerUserAndDateRangeAsync(seller, startDate, endDate);

        public Task DeleteAllByUserAsync(Account user) => _cartItemDAO.DeleteAllByUserAsync(user);

        public Task DeleteCartItem(CartItem cartItem) => _cartItemDAO.DeleteCartItem(cartItem);

        public Task<IEnumerable<CartItem>> GetAllByUserAsync(Account user) => _cartItemDAO.GetAllByUserAsync(user);

        public Task<IEnumerable<CartItem>> GetAllCartItemUserIdAsync(string user) => _cartItemDAO.GetAllCartItemUserIdAsync(user);

        public Task<CartItem> GetByUserAndFlowerAsync(string user, string flowerID) => _cartItemDAO.GetByUserAndFlowerAsync(user, flowerID);

        public Task<bool> UpdateCartItem(CartItem cartItem) => _cartItemDAO.UpdateCartItem(cartItem);
    }
}
