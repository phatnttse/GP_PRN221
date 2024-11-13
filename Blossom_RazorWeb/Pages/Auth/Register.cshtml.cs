using Blossom_BusinessObjects;
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
        private readonly INotificationService _notificationService;
        private readonly EmailSender _emailSender;
        public RegisterModel(IAccountService accountService, EmailSender emailSender, INotificationService notificationService)
        {
            _accountService = accountService;
            _emailSender = emailSender;
            _notificationService = notificationService;
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
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "sendVerifyEmail.html");

                    // Prepare placeholders for the template
                    var placeholders = new Dictionary<string, string>
                    {
                        { "name", FullName },
                        { "link", "https://yourdomain.com/confirm?token=abc123" }
                    };

                    string emailBody = EmailTemplateHelper.GetEmailBody(templatePath, placeholders);
                    await _emailSender.SendEmailAsync(Email, "Account register successfully", emailBody);
                    var user = await _accountService.GetAccount(Email);
                    var notification = new Notification()
                    {
                        Title = $"Welcome {FullName} to your platform",
                        Message = "Feel free to look around our app!",
                        Type = NotificationTypeEnum.WELCOME,
                        DestinationScreen = DestinationScreenEnum.HOME,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsRead = false,
                        ReceiverId = user.Id,
                    };
                    await _notificationService.PushNotificationAsync(notification);
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
