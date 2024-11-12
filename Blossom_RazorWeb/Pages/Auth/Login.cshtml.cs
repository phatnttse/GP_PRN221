using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _accountService.Login(Email, Password);

                if (result)
                {
                    var account = await _accountService.GetAccount(Email);

                    if (account != null)
                    {
                        HttpContext.Session.SetString("Avatar", account.Avatar);
                        HttpContext.Session.SetString("AccountId", account.Id);

                        var roles = await _accountService.GetRoles(account);
                        HttpContext.Session.SetString("Role", roles[0]);

                        TempData["LoginSuccessMessage"] = "Đăng nhập thành công!";

                        return RedirectToPage("/Index");
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page(); 
            }
        }
    }
}
