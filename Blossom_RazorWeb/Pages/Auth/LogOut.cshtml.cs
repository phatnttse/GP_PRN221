using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Auth
{
    public class LogOutModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LogOutModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public void OnGet()
        {
            _accountService.Logout();
            Response.Redirect("/Index");
        }
    }
}
