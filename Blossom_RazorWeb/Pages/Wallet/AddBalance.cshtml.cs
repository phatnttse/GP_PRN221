using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_Services.Interfaces;

namespace Blossom_RazorWeb.Pages.Wallet
{
    public class AddBalanceModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IUserIdAssessor _userIdAssessor;

        public AddBalanceModel(IPaymentService paymentService, IUserIdAssessor userIdAssessor)
        {
            _paymentService = paymentService;
            _userIdAssessor = userIdAssessor;
        }

        [BindProperty]
        public long Amount { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Amount <= 0)
                {
                    ErrorMessage = "Số tiền không hợp lệ.";
                    return Page();
                }

                var userId = _userIdAssessor.GetCurrentUserId();
                var payUrl = await _paymentService.CreateMomoPayment(Amount, userId);

                if (string.IsNullOrEmpty(payUrl))
                {
                    ErrorMessage = "Không thể tạo thanh toán Momo.";
                    return Page();
                }

                return Redirect(payUrl);

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            
        }
    }
}
