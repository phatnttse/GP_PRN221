using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Auth
{
    public class RegisterModel : PageModel
    {

        private readonly IAccountService _accountService;
        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public Gender? Gender { get; set; }
        public string ErrorMessage { get; private set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    return Page();
                }

                if (Gender == null)
                {
                    ErrorMessage = "Gender is required.";
                    return Page();
                }

                var response = await _accountService.RegisterAccountAsync(FullName , Email, Password, Gender.Value);

                if (response)
                {
                    TempData["RegisterSuccessMessage"] = "Đăng ký tài khoản thành công. Đăng nhập ngay !";
                    return RedirectToPage("/Auth/Login");
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

        }
    }
}
