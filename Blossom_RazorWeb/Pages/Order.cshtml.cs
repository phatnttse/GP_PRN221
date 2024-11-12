using Blossom_BusinessObjects;
using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Blossom_BusinessObjects.Enums;

namespace Blossom_RazorWeb.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IFlowerService _flowerService;
        private readonly ICartItemService _cartItemService;
        private readonly IAccountService _accountService;
        private readonly IWalletLogService _walletLogService;

        public OrderModel(
            ICartItemService cartItemService,
            IAccountService accountService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IFlowerService flowerService,
            IWalletLogService walletLogService
        )
        {
            _cartItemService = cartItemService;
            _accountService = accountService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _flowerService = flowerService;
            _walletLogService = walletLogService;
        }

        [BindProperty]
        public Order Order { get; set; } = new Order();

        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Flower> Flowers { get; set; } = new List<Flower>();
        public Flower Flower { get; set; }
        public Account Account { get; set; }

        public WalletLog WalletLog { get; set; }
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
            Account account = await _accountService.GetAccountById(userId);
            if (account != null)
            {
                Order = new Order
                {
                    BuyerName = account.FullName,
                    BuyerPhone = account.PhoneNumber,
                    BuyerEmail = account.Email,
                    BuyerAddress = account.Address
                };
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

        public async Task<IActionResult> OnPostAsync()
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

                // Get the payment method from the form
                var paymentMethod = Request.Form["paymentMethod"];
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    ModelState.AddModelError("", "Please select a payment method.");
                    return Page();
                }

                // Parse the payment method to the enum
                if (!Enum.TryParse(paymentMethod, out PaymentMethodEnum selectedPaymentMethod))
                {
                    ModelState.AddModelError("", "Invalid payment method selected.");
                    return Page();
                }

                // Create a new order
                Order.UserId = userId;
                Order.TotalPrice = CartItems.Sum(item => item.Flower.Price * item.Quantity);
                Order.CreatedAt = DateTime.Now;
                Order.Status = OrderStatus.PENDING;

                // Add the order to the database and ensure the order gets an ID
                var createdOrder = _orderService.AddOrder(Order);
                if (createdOrder == null)
                {
                    ModelState.AddModelError("", "Failed to create the order. Please try again.");
                    return Page();
                }

                // List to store flowers that need stock updates
                var flowersToUpdate = new List<Flower>();

                foreach (var cartItem in CartItems)
                {
                    if (cartItem?.Flower != null)
                    {
                        var flower = await _flowerService.GetFlower(cartItem.Flower.Id);
                        if (flower == null)
                        {
                            TempData["Error"] = $"Flower with ID {cartItem.Flower.Id} not found.";
                            return Page();
                        }

                        if (flower.StockQuantity < cartItem.Quantity)
                        {
                            TempData["Error"] = $"Not enough stock for flower '{flower.Name}'.";
                            return Page();
                        }

                        flower.StockQuantity -= cartItem.Quantity;
                        flowersToUpdate.Add(flower);

                        var orderDetail = new OrderDetail
                        {
                            OrderId = Order.Id,
                            SellerId = flower.SellerId,
                            FlowerId = flower.Id,
                            Price = flower.Price,
                            Quantity = cartItem.Quantity,
                            PaymentMethod = selectedPaymentMethod,
                            Status = OrderDetailStatus.PENDING
                        };

                        // Add the order details
                        var createdOrderDetail = _orderDetailService.AddOrderDetail(orderDetail);
                        if (!createdOrderDetail)
                        {
                            TempData["Error"] = "Failed to create order details.";
                            return Page();
                        }
                    }
                }

                // Update flowers stock in the database
                foreach (var flower in flowersToUpdate)
                {
                    await _flowerService.UpdateFlower(flower);
                }

                // Optionally, process payment here based on the payment method
                // For example, if paymentMethod is "Wallet", perform wallet payment processing

                return RedirectToPage("OrderSuccessModel", new { orderId = Order.Id });
            }
            catch (Exception ex)
            {
                // Log the exception and notify the user
                TempData["Error"] = ex.Message;
                return Page();
            }
        }
    }
}
