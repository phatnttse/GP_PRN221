using Blossom_BusinessObjects;
using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Wallet
{
    public class IndexModel : PageModel
    {
        private readonly IUserIdAssessor _userIdAssessor;
        private readonly IAccountService _accountService;
        private readonly IWalletLogService _walletLogService;

        public IndexModel(IUserIdAssessor userIdAssessor, IAccountService accountService, IWalletLogService walletLogService)
        {
            _userIdAssessor = userIdAssessor;
            _accountService = accountService;
            _walletLogService = walletLogService;
        }

        public List<WalletLog> WalletLogs { get; set; } = new List<WalletLog>();
        public decimal accountBalance { get; set; }

        public void OnGet()
        {
            var accountId = _userIdAssessor.GetCurrentUserId();
            Account account = _accountService.GetAccountById(accountId).Result;
            accountBalance = account.Balance;
            WalletLogs = _walletLogService.GetByUserId(accountId);
        }
    }
}
