using Blossom_BusinessObjects.Entities;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages
{
    public class CartItemModel : PageModel
    {
        private readonly ICartItemService _cartItemService;
        private readonly IUserIdAssessor _userIdAccessor;
        private readonly IFlowerService _flowerService;
        private readonly IAccountService _accountService;


        public CartItemModel(ICartItemService cartItemService, IUserIdAssessor userIdAccessor, IFlowerService flowerService, IAccountService accountService)
        {
            _cartItemService = cartItemService;
            _userIdAccessor = userIdAccessor;
            _flowerService = flowerService;
            _accountService = accountService;
        }

        public IEnumerable<CartItem> CartItem { get; set; } = default!;
        public IList<Flower> Flower { get; set; } = default!;

        public decimal TotalPrice { get; set; }


        public async Task OnGetAsync()
        {
            var currentUser =  _userIdAccessor.GetCurrentUserId();
            if (currentUser != null)
            {
                CartItem = (await _cartItemService.GetAllCartItemUserIdAsync(currentUser)).ToList();
                TotalPrice = CartItem.Sum(item => item.Quantity * item.Flower.Price);
            }

        }

        public async Task<IActionResult> OnPostIncreaseCartItems(string flowerId, int quantity)
        {
            try
            {
                var currentUser = _userIdAccessor.GetCurrentUserId();
                var cartItem = await _cartItemService.GetByUserAndFlowerAsync(currentUser, flowerId);
                var flowerQuantity = await _flowerService.GetFlower(flowerId);

                if (cartItem.Quantity <= flowerQuantity.StockQuantity)
                {
                    await _cartItemService.AddFlowerListingToCart(currentUser, flowerId, 1);
                }
                if (cartItem.Quantity >= flowerQuantity.StockQuantity)
                {
                    cartItem.Quantity = flowerQuantity.StockQuantity;
                    await _cartItemService.AddFlowerListingToCart(currentUser, flowerId, 0);
                }
                return RedirectToPage("/CartItem");
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDecreaseCartItems(string flowerId, int quantity)
        {
            try
            {
                var currentUser = _userIdAccessor.GetCurrentUserId();
                var cartItem = await _cartItemService.GetByUserAndFlowerAsync(currentUser, flowerId);

                if (cartItem.Quantity - 1 <= 0)
                {
                    await _cartItemService.DeleteCartItem(currentUser, flowerId);
                    return RedirectToPage("/CartItem");
                }

                await _cartItemService.AddFlowerListingToCart(currentUser, flowerId, -1);

                return RedirectToPage("/CartItem");
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteCartItems(string flowerId)
        {
            try
            {
                var currentUser = _userIdAccessor.GetCurrentUserId();
                await _cartItemService.DeleteCartItem(currentUser, flowerId);

                return RedirectToPage("/CartItem");
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostClearCart()
        {
            try
            {
                var currentUserId = _userIdAccessor.GetCurrentUserId();
                Account account = _accountService.GetAccountById(currentUserId).Result;

                await _cartItemService.DeleteAllByUserAsync(account);

                return RedirectToPage("/CartItem");
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }


    }
}
