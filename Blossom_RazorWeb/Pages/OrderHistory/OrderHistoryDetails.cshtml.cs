using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_BusinessObjects.Enums;
using Blossom_BusinessObjects;

namespace Blossom_RazorWeb.Pages.OrderHistory
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IUserIdAssessor _userIdAssessor;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IWalletLogService _walletLogService;

        public OrderHistoryModel(
            IOrderService orderService, 
            IUserIdAssessor userIdAssessor, 
            IOrderDetailService orderDetailService,
            IWalletLogService walletLogService
            )
        {
            _orderService = orderService;
            _userIdAssessor = userIdAssessor;
            _orderDetailService = orderDetailService;
            _walletLogService = walletLogService;
        }

        public List<OrderDetail> OrderDetails { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public WalletLog WalletLog { get; set; }
        public int TotalPages { get; set; }
        public const int PageSize = 3;
        public void OnGet()
        {
            var existingAccount = _userIdAssessor.GetCurrentUserId();

            if (existingAccount != null)
            {
                OrderDetails = _orderDetailService.GetOrderDetailsById(existingAccount);
                TotalPages = (int)Math.Ceiling(OrderDetails.Count() / (double)PageSize);
                OrderDetails = OrderDetails.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(string orderDetailId)
        {
            var existingAccount = _userIdAssessor.GetCurrentUserId();
            if (existingAccount == null)
            {
                return Unauthorized();
            }

            var orderDetail = _orderDetailService.GetOrderDetail(orderDetailId);

            

            if (orderDetail != null &&
                (orderDetail.Status == OrderDetailStatus.PENDING ))
            {
                orderDetail.Status = OrderDetailStatus.CANCELED;
                _orderDetailService.UpdateOrderDetail(orderDetail);


                decimal feeService = 5 / 100;
                decimal calAmountForUser = orderDetail.Price;
                decimal calAmoutnForSeller = calAmountForUser * feeService;
                //handle balance for user
                //handle balance for seller
                var buyerLog = CreateWalletLog(existingAccount, calAmountForUser, WalletLogTypeEnum.ADD, WalletLogActorEnum.BUYER, 0);
                var sellerLog = CreateWalletLog(orderDetail.SellerId, calAmoutnForSeller, WalletLogTypeEnum.ADD, WalletLogActorEnum.SELLER, 0);
                _walletLogService.Create(buyerLog);
                _walletLogService.Create(sellerLog);
                TempData["Success"] = "Đơn hàng đã được hủy thành công.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeliveredOrder(string orderDetailId)
        {
            var existingAccount = _userIdAssessor.GetCurrentUserId();
            if (existingAccount == null)
            {
                return Unauthorized();
            }

            var orderDetail = _orderDetailService.GetOrderDetail(orderDetailId);
            if (orderDetail != null &&
                (orderDetail.Status == OrderDetailStatus.SHIPPED))
            {
                orderDetail.Status = OrderDetailStatus.DELIVERED;
                _orderDetailService.UpdateOrderDetail(orderDetail);
                TempData["Success"] = "Cảm ơn bạn đã xác nhận đơn hàng!";
            }
            return RedirectToPage();
        }
        private WalletLog CreateWalletLog(string userId, decimal amount, WalletLogTypeEnum type, WalletLogActorEnum actor, decimal currentBalance) => new WalletLog
        {
            UserId = userId,
            Amount = amount,
            Type = type,
            Status = WalletLogStatusEnum.SUCCESS,
            ActorEnum = actor,
            IsRefund = true,
            Balance = currentBalance,
            PaymentMethod = PaymentMethodEnum.WALLET,
            CreatedAt = DateTime.Now
        };
    }
}
