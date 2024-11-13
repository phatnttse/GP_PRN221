using Azure;
using Blossom_BusinessObjects.Entities;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFlowerService _flowerService;
        private readonly IFlowerCategoryService _categoryService;
        private readonly ICartItemService _cartItemService;
        private readonly IUserIdAssessor _userIdAccessor;
        public List<FlowerCategory> Categories { get; set; }
        public string TitleSearch { get; set; } = "Our Products";
        public string SearchString { get; set; }
        public int CurrentPage { get; set; } = 0;
        public int TotalPages { get; set; } = 1;

        public IndexModel(ILogger<IndexModel> logger, IFlowerService flowerService, IFlowerCategoryService categoryService, ICartItemService cartItemService, IUserIdAssessor userIdAccessor)
        {
            _logger = logger;
            _flowerService = flowerService;
            _categoryService = categoryService;
            _cartItemService = cartItemService;
            _userIdAccessor = userIdAccessor;
        }

        public List<Flower> Flowers { get; set; }

        public void OnGet(string search = "", int page = 0)
        {
            Categories = _categoryService.GetFlowerCategories().Result;
            var flowerList = _flowerService.GetFlowers().Result;

            // Filtering logic
            if (!string.IsNullOrEmpty(search))
            {
                flowerList = flowerList.Where(f => f.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Pagination logic
            const int pageSize = 12;
            Flowers = flowerList.Skip(page * pageSize).Take(pageSize).ToList();
            TotalPages = (int)Math.Ceiling(flowerList.Count / (double)pageSize);
            CurrentPage = page;
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
                        return RedirectToPage("/Products/Index");
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
