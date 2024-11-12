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
        public string Message { get; set; }


        public async void OnGet()
        {
            Flowers = await _flowerService.GetFlowers();
        }

        public async Task<IActionResult> OnPostAddToCart(string flowerId, int quantity)
        {
            try
            {
                var currentUser = _userIdAccessor.GetCurrentUserId();
                // Giả sử bạn có phương thức AddFlowerListingToCart trong service
                if (currentUser != null)
                {
                    await _cartItemService.AddFlowerListingToCart(currentUser, flowerId, 1);

                    // Reload danh sách hoa sau khi thêm vào giỏ hàng
                    Flowers = await _flowerService.GetFlowers();
                    // Có thể bạn cần điều hướng người dùng đến giỏ hàng hoặc trang khác
                    TempData["Message"] = "Hoa đã được thêm vào giỏ hàng!";
                    return RedirectToPage("/CartItem");
                }
                else
                {
                    TempData["LoginFailMessage"] = "Vui lòng đăng nhập!";
                    return RedirectToPage("/Index");
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
