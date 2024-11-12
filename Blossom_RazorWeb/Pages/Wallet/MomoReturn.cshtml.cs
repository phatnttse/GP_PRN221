using Blossom_BusinessObjects;
using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Wallet
{
    public class MomoReturnModel : PageModel
    {
        private readonly IUserIdAssessor _userIdAssessor;
        private readonly IAccountService _accountService;
        private readonly IWalletLogService _walletLogService;

        public MomoReturnModel(IUserIdAssessor userIdAssessor, IAccountService accountService, IWalletLogService walletLogService)
        {
            _userIdAssessor = userIdAssessor;
            _accountService = accountService;
            _walletLogService = walletLogService;
        }
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public string Amount { get; set; }
        public string OrderInfo { get; set; }
        public string OrderType { get; set; }
        public string TransId { get; set; }
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string PayType { get; set; }
        public string ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
        public int statusPayment { get; set; } = 0; // 0: Chưa xử lý, 1: Thành công, 2: Thất bại

        public void OnGet()
        {
            try
            {
                PartnerCode = Request.Query["partnerCode"];
                OrderId = Request.Query["orderId"];
                RequestId = Request.Query["requestId"];
                Amount = Request.Query["amount"];
                OrderInfo = Request.Query["orderInfo"];
                OrderType = Request.Query["orderType"];
                TransId = Request.Query["transId"];
                ResultCode = Request.Query["resultCode"];
                Message = Request.Query["message"];
                PayType = Request.Query["payType"];
                ResponseTime = Request.Query["responseTime"];
                ExtraData = Request.Query["extraData"];
                Signature = Request.Query["signature"];

                var accountId = _userIdAssessor.GetCurrentUserId();
                var amount = long.Parse(Amount);
                Account account = _accountService.GetAccountById(accountId).Result;

                if (ResultCode == "0")
                {
                    var newBalance = account.Balance += amount;
                    account.Balance = newBalance;

                    WalletLog walletLog = new WalletLog
                    {
                        UserId = accountId,
                        Amount = amount,
                        Type = WalletLogTypeEnum.ADD,
                        Balance = newBalance,
                        ActorEnum = WalletLogActorEnum.BUYER,
                        Status = WalletLogStatusEnum.SUCCESS,
                        PaymentMethod = PaymentMethodEnum.WALLET,
                        IsRefund = false,
                    };

                    _walletLogService.Create(walletLog);
                    _accountService.UpdateAccount(account);

                }
                else if (ResultCode == "1006")
                {
                    
                        WalletLog walletLog = new WalletLog
                        {
                            UserId = accountId,
                            Amount = amount,
                            Type = WalletLogTypeEnum.ADD,
                            Balance = account.Balance,
                            ActorEnum = WalletLogActorEnum.BUYER,
                            Status = WalletLogStatusEnum.PENDING,
                            PaymentMethod = PaymentMethodEnum.WALLET,
                            IsRefund = false,
                        };

                        _walletLogService.Create(walletLog);
                    
                }

                Response.Redirect("/Wallet");

            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
          
        }
    }
}
