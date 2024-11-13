using Blossom_BusinessObjects.Entities;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFlowerService _flowerService;
        private readonly ICartItemService _cartItemService;
        private readonly IUserIdAssessor _userIdAccessor;

        public IndexModel(
            ILogger<IndexModel> logger
            , IFlowerService flowerService
            , ICartItemService cartItemService,
IUserIdAssessor userIdAccessor)
        {
            _logger = logger;
            _flowerService = flowerService;
            _cartItemService = cartItemService;
            _userIdAccessor = userIdAccessor;
        }

        public List<Flower> Flowers { get; set; }
        public string ErrorMessage { get; set; }


        public async void OnGet()
        {
            Flowers = await _flowerService.GetFlowers();
        }

        public async Task<IActionResult> OnPostAddToCart(string flowerId, int quantity)
        {
            try
            {
                var currentUserId = _userIdAccessor.GetCurrentUserId();
               
                if (currentUserId != null)
                {
                    var cartItem = await _cartItemService.GetByUserAndFlowerAsync(currentUserId, flowerId);
                    var flowerQuantity = await _flowerService.GetFlower(flowerId);
                    if (cartItem != null)
                    {
                        if (cartItem.Quantity <= flowerQuantity.StockQuantity)
                        {
                            await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, 1);
                        }
                        if (cartItem.Quantity >= flowerQuantity.StockQuantity)
                        {
                            cartItem.Quantity = flowerQuantity.StockQuantity;
                            await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, 0);
                        }

                        return RedirectToPage("/CartItem");

                    }
                    else 
                    {
                        await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, 1);
                        return RedirectToPage("/Index"); 
                    }

                }
                else
                {
                    TempData["NotPermissionMessage"] = "Vui lòng đăng nhập!";
                    return RedirectToPage("/Auth/Login");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding flower to cart");
                return RedirectToPage();
            }
        }
    }
}
