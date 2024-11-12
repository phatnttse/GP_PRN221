using Blossom_BusinessObjects.Entities;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IFlowerRepository _flowerRepository;
        public CartItemService(ICartItemRepository cartItemRepository, IFlowerRepository flowerRepository) {
            _cartItemRepository = cartItemRepository;   
            _flowerRepository = flowerRepository;
        }

        public async Task AddFlowerListingToCart(string user, string flowerId, int quantity)
        {
            var existingCartItem = await _cartItemRepository.GetByUserAndFlowerAsync(user, flowerId);
            var flowerExisting = await _flowerRepository.GetFlower(flowerId);

            if (existingCartItem != null)
            {
                // Update quantity if item already in cart
                existingCartItem.Quantity += quantity;
                _cartItemRepository.UpdateCartItem(existingCartItem);
            } else
            {
                // Nếu chưa tồn tại, tạo mới CartItem và thêm vào
                var newCartItem = new CartItem
                {
                    UserId = user,
                    FlowerId = flowerId,
                    Quantity = quantity > flowerExisting.StockQuantity ? flowerExisting.StockQuantity : quantity,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _cartItemRepository.AddFlowerListingToCart(user, newCartItem);
            }

        }

        public Task<int> CountCartItemsByFlowerUserAndDateRangeAsync(Account seller, DateTime startDate, DateTime endDate) => _cartItemRepository.CountCartItemsByFlowerUserAndDateRangeAsync(seller, startDate, endDate);

        public Task DeleteAllByUserAsync(Account user) => _cartItemRepository.DeleteAllByUserAsync(user);

        public async Task DeleteCartItem(string user, string flowerId)
        {
            var existingCartItem = await _cartItemRepository.GetByUserAndFlowerAsync(user, flowerId);

            if (existingCartItem != null)
            {
                // Update quantity if item already in cart
                existingCartItem.Quantity = 0;
                _cartItemRepository.DeleteCartItem(existingCartItem);
            }
            else
            {
                _cartItemRepository.AddFlowerListingToCart(user, existingCartItem);
            }
        }
        public Task<IEnumerable<CartItem>> GetAllByUserAsync(Account user) => _cartItemRepository.GetAllByUserAsync(user);

        public Task<IEnumerable<CartItem>> GetAllCartItemUserIdAsync(string user)=> _cartItemRepository.GetAllCartItemUserIdAsync(user);

        public Task<CartItem> GetByUserAndFlowerAsync(string user, string flowerID) => _cartItemRepository.GetByUserAndFlowerAsync(user, flowerID);

        public async Task ReduceFlowerListingQuantityInCart(string user, string flowerId, int quantity)
        {
            var existingCartItem = await _cartItemRepository.GetByUserAndFlowerAsync(user, flowerId);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity > quantity)
                {
                    existingCartItem.Quantity -= quantity;
                    _cartItemRepository.UpdateCartItem(existingCartItem);
                }
            }
        }

        public Task<bool> UpdateCartItem(CartItem cartItem) => _cartItemRepository.UpdateCartItem(cartItem);
    }
}
