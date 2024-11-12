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

        [BindProperty]
        public Order Order { get; set; } = new Order();

        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public IEnumerable<CartItem> CartItems { get; set; }
        public List<Flower> Flowers { get; set; }
        public Account Account { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderModel(IOrderService orderService, IOrderDetailService orderDetailService, IFlowerService flowerService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _flowerService = flowerService;
        }

        public async Task OnGetAsync()
        {
            // Initialize CartItems
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var account = await _accountService.GetAccount(userEmail);
            CartItems = account != null
                ? await _cartItemService.GetAllByUserAsync(account)
                : new List<CartItem>();
            CartItems = new List<CartItem>
            {
                new CartItem
                {
                    FlowerId = "sdfsdafasfd",
                    Flower = _flowerService.GetFlower("sdfsdafasfd").Result,
                    Quantity = 2
                },
                new CartItem
                {
                    FlowerId = "sdfsdafasfda",
                    Flower = _flowerService.GetFlower("sdfsdafasfda").Result,
                    Quantity = 1
                }
            };

            if (CartItems == null)
            {
                CartItems = new List<CartItem>();
            }

            Flowers = _flowerService.GetFlowers().Result;
            TotalPrice = 0;

            foreach (var detail in CartItems)
            {
                if (detail?.Flower != null)
                {
                    TotalPrice += detail.Flower.Price * detail.Quantity;
                }
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        FlowerId = "sdfsdafasfd",
                        Flower = _flowerService.GetFlower("sdfsdafasfd").Result,
                        Quantity = 2
                    },
                    new CartItem
                    {
                        FlowerId = "sdfsdafasfda",
                        Flower = _flowerService.GetFlower("sdfsdafasfda").Result,
                        Quantity = 1
                    }
                };

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "User is not authenticated");
                    return Page();
                }

                Order.UserId = userId;
                Order.TotalPrice = TotalPrice;
                Order.CreatedAt = DateTime.Now;
                Order.Status = OrderStatus.PENDING;

                _orderService.AddOrder(Order);

                foreach (var cartItem in CartItems)
                {
                    if (cartItem?.Flower != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = Order.Id,
                            SellerId = cartItem.Flower.SellerId,
                            FlowerId = cartItem.Flower.Id,
                            Price = cartItem.Flower.Price,
                            Quantity = cartItem.Quantity,
                            Status = OrderDetailStatus.PENDING
                        };
                        _orderDetailService.AddOrderDetail(orderDetail);
                    }
                }

                return RedirectToPage("OrderSuccessModel", new { orderId = Order.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                return Page();
            }
        }
    }
}
