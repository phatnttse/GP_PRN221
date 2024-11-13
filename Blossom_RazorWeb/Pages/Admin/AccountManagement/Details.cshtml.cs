using Blossom_BusinessObjects.Entities;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.AccountManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;

        public Account Account { get; set; }
        public IList<string> Role { get; set; }
        public DetailsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Account = await _accountService.GetAccountById(id);
            Role = await _accountService.GetRoles(Account);
            if (Account == null)
            {
                return NotFound();
            }

            return Page();
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

            return RedirectToPage(new { id = accountId });
        }
    }
}
