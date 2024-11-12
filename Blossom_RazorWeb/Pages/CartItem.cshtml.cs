using Blossom_BusinessObjects.Entities;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages
{
    public class CartItemModel : PageModel
    {
        private readonly ICartItemService _context;
        private readonly IUserIdAssessor _userIdAccessor;
        private readonly IFlowerService _flowerService;


        public CartItemModel(ICartItemService context, IUserIdAssessor userIdAccessor, IFlowerService flowerService)
        {
            _context = context;
            _userIdAccessor = userIdAccessor;
            _flowerService = flowerService;
        }

        public IEnumerable<CartItem> CartItem { get; set; } = default!;
        public IList<Flower> Flower { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var currentUser =  _userIdAccessor.GetCurrentUserId();
            //var currentUser = "03a7b561-c1c1-448e-924e-9d7ac13a1352";
            if (currentUser != null)
            {
                CartItem = (await _context.GetAllCartItemUserIdAsync(currentUser)).ToList();
            }

            Flower = await _flowerService.GetFlowers();
        }

        public async Task<IActionResult> OnPostIncreaseCartItems(string flowerId, int quantity)
        {
            try
            {
                var currentUser = _userIdAccessor.GetCurrentUserId();
                // Gi? s? b?n c� ph??ng th?c AddFlowerListingToCart trong service
                await _context.AddFlowerListingToCart(currentUser, flowerId, 1);

                // C� th? b?n c?n ?i?u h??ng ng??i d�ng ??n gi? h�ng ho?c trang kh�c
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
                // Gi? s? b?n c� ph??ng th?c AddFlowerListingToCart trong service
                await _context. ReduceFlowerListingQuantityInCart(currentUser, flowerId, 1);

                // C� th? b?n c?n ?i?u h??ng ng??i d�ng ??n gi? h�ng ho?c trang kh�c
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
                // Gi? s? b?n c� ph??ng th?c AddFlowerListingToCart trong service
                await _context.DeleteCartItem(currentUser, flowerId);

                // C� th? b?n c?n ?i?u h??ng ng??i d�ng ??n gi? h�ng ho?c trang kh�c
                return RedirectToPage("/CartItem");
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }


    }
}
