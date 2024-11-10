using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
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
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.FullName),
                            new Claim(ClaimTypes.Email, account.Email),
                            new Claim(ClaimTypes.NameIdentifier, account.Id),
                            new Claim("Avatar", account.Avatar)
                        };
                        HttpContext.Session.SetString("Avatar", account.Avatar);

                        var roles = await _accountService.GetRoles(account);

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                     

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
