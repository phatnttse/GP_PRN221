using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blossom_RazorWeb.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IFlowerService _flowerService;
        private readonly ICartItemService _cartItemService;
        private readonly IAccountService _accountService;

        public OrderModel(
            ICartItemService cartItemService,
            IAccountService accountService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IFlowerService flowerService)
        {
            _cartItemService = cartItemService;
            _accountService = accountService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _flowerService = flowerService;
        }

        [BindProperty]
        public Order Order { get; set; } = new Order();

        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Flower> Flowers { get; set; } = new List<Flower>();
        public Flower Flower { get; set; }
        public Account Account { get; set; }

        public decimal TotalPrice { get; set; }

        // GET method to initialize data
        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                TempData["Error"] = "You do not have permission to do this function!";
                return RedirectToPage("/Auth/Login");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                CartItems = (await _cartItemService.GetAllCartItemUserIdAsync(userId)).ToList();
            }

            Flowers = await _flowerService.GetFlowers();
            TotalPrice = 0;

            // Calculate total price
            foreach (var detail in CartItems)
            {
                if (detail?.Flower != null)
                {
                    TotalPrice += detail.Flower.Price * detail.Quantity;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "User is not authenticated");
                    return Page();
                }

                // Fetch user's cart items
                CartItems = (await _cartItemService.GetAllCartItemUserIdAsync(userId)).ToList();

                if (CartItems == null || !CartItems.Any())
                {
                    ModelState.AddModelError("", "No items in the cart");
                    return Page();
                }

                // Create a new order
                Order.UserId = userId;
                Order.TotalPrice = CartItems.Sum(item => item.Flower.Price * item.Quantity);
                Order.CreatedAt = DateTime.Now;
                Order.Status = OrderStatus.PENDING;

                _orderService.AddOrder(Order);

                // List to store flowers that need stock updates
                var flowersToUpdate = new List<Flower>();

                foreach (var cartItem in CartItems)
                {
                    if (cartItem?.Flower != null)
                    {
                        // Fetch the flower from the database
                        var flower = await _flowerService.GetFlower(cartItem.Flower.Id);

                        if (flower == null)
                        {
                            TempData["Error"] = $"Flower with ID {cartItem.Flower.Id} not found.";
                            return Page();
                        }

                        // Check if there's enough stock
                        if (flower.StockQuantity < cartItem.Quantity)
                        {
                            TempData["Error"] = $"Not enough stock for flower '{flower.Name}'.";
                            return Page();
                        }

                        // Deduct the stock quantity
                        flower.StockQuantity -= cartItem.Quantity;
                        flowersToUpdate.Add(flower);

                        // Create order detail
                        var orderDetail = new OrderDetail
                        {
                            OrderId = Order.Id,
                            SellerId = flower.SellerId,
                            FlowerId = flower.Id,
                            Price = flower.Price,
                            Quantity = cartItem.Quantity,
                            Status = OrderDetailStatus.PENDING
                        };

                        _orderDetailService.AddOrderDetail(orderDetail);
                    }
                }


                // Update flower stocks after successful transaction
                foreach (var flower in flowersToUpdate)
                {
                    await _flowerService.UpdateFlower(flower);
                }



                return RedirectToPage("OrderSuccessModel", new { orderId = Order.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your order. Please try again.";
                return Page();
            }
        }
    }
}
