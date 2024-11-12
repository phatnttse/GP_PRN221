using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_BusinessObjects.Enums;
using Blossom_BusinessObjects;
using Blossom_Services;

namespace Blossom_RazorWeb.Pages.OrderHistory
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IUserIdAssessor _userIdAssessor;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IWalletLogService _walletLogService;
        private readonly IAccountService _accountService;

        public OrderHistoryModel(
            IOrderService orderService, 
            IUserIdAssessor userIdAssessor, 
            IOrderDetailService orderDetailService,
            IWalletLogService walletLogService,
            IAccountService accountService
            )
        {
            _orderService = orderService;
            _userIdAssessor = userIdAssessor;
            _orderDetailService = orderDetailService;
            _walletLogService = walletLogService;
            _accountService = accountService;
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

            if (orderDetail.PaymentMethod == PaymentMethodEnum.COD)
            {
                TempData["Error"] = $"Phương thức thanh toán bởi SHIP {orderDetail.PaymentMethod} chưa hỗ trợ hủy!";
            }

            if (orderDetail.PaymentMethod == PaymentMethodEnum.WALLET)
            {
                if (orderDetail != null &&
                (orderDetail.Status == OrderDetailStatus.PENDING))
                {
                    Account seller = await _accountService.GetAccountById(orderDetail.SellerId);
                    Account user = await _accountService.GetAccountById(existingAccount);

                    orderDetail.Status = OrderDetailStatus.CANCELED;
                    _orderDetailService.UpdateOrderDetail(orderDetail);


                    decimal feeService = 5 / 100;
                    decimal calAmountForUser = orderDetail.Price;
                    decimal calAmoutnForSeller = calAmountForUser * feeService;
                    //handle balance for user
                    user.Balance = user.Balance + calAmountForUser;
                    seller.Balance = seller.Balance - calAmoutnForSeller;

                    //handle balance for user 
                    Account updateBalanceUser = await _accountService.UpdateAccount(user);
                    //handle balance for seller
                    Account updateBalanceSeller = await _accountService.UpdateAccount(seller);
                    //handle balance for seller
                    var buyerLog = CreateWalletLog(existingAccount, calAmountForUser, WalletLogTypeEnum.ADD, WalletLogActorEnum.BUYER, updateBalanceUser.Balance);
                    var sellerLog = CreateWalletLog(orderDetail.SellerId, calAmoutnForSeller, WalletLogTypeEnum.SUBTRACT, WalletLogActorEnum.SELLER, updateBalanceSeller.Balance);
                    _walletLogService.Create(buyerLog);
                    _walletLogService.Create(sellerLog);
                    TempData["Success"] = "Đơn hàng đã được hủy thành công.";
                }
                
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
