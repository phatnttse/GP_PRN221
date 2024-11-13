using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Blossom_RazorWeb.Pages.Admin.AccountManagement
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public List<Account> Accounts { get; set; }

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            var allAccounts = await _accountService.GetAccounts();

            var currentAccountId = HttpContext.Session.GetString("AccountId");

            Accounts = allAccounts
                .Where(account => account.Id != currentAccountId)
                .ToList();
        }

        public async Task<IActionResult> OnPostBanUnbanAsync(string accountId, bool isBan)
        {
            var account = await _accountService.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound();
            }

            account.LockoutEnabled = isBan;
            account.LockoutEnd = isBan ? DateTimeOffset.MaxValue : (DateTimeOffset?)null;
            await _accountService.UpdateAccount(account);

            return RedirectToPage();
        }
    }
}
