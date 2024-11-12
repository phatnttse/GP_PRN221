using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blossom_RazorWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartItemService _cartItemService;
        public CartController(ICartItemService cartItemService)
        {
                _cartItemService = cartItemService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(string user, string flowerId, int quantity)
        {
            // Thêm sản phẩm vào giỏ hàng
            _cartItemService.AddFlowerListingToCart("03a7b561-c1c1-448e-924e-9d7ac13a1352", flowerId, 1);

            // Sau khi thêm sản phẩm, điều hướng lại trang giỏ hàng
            return RedirectToAction("Index");
        }

    }
}
