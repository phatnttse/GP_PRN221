using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Blossom_Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Auth
{
    public class RegisterModel : PageModel
    {

        private readonly IAccountService _accountService;
        private readonly EmailSender _emailSender;
        public RegisterModel(IAccountService accountService, EmailSender emailSender)
        {
            _accountService = accountService;
            _emailSender = emailSender;
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
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "sendVerifyEmail.html");

                    // Prepare placeholders for the template
                    var placeholders = new Dictionary<string, string>
                    {
                        { "name", FullName }, // User's name
                        { "link", "https://yourdomain.com/confirm?token=abc123" }
                    };

                    string emailBody = EmailTemplateHelper.GetEmailBody(templatePath, placeholders);
                    await _emailSender.SendEmailAsync(Email, "Account register successfully", emailBody);
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
