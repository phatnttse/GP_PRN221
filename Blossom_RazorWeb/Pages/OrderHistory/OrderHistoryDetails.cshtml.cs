using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.OrderHistory
{
    public class OrderHistoryModel : PageModel
    {
        private IOrderService _orderService;
        private IUserIdAssessor _userIdAssessor;
        private IOrderDetailService _orderDetailService;

        public OrderHistoryModel(IOrderService orderService, IUserIdAssessor userIdAssessor, IOrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _userIdAssessor = userIdAssessor;
            _orderDetailService = orderDetailService;
        }
        public List<OrderDetail> OrderDetails { get; set; }
        public async void OnGetAsync()
        {
            var existingAccount = _userIdAssessor.GetCurrentUserId();
            if (existingAccount != null)
            {
                OrderDetails = _orderDetailService.GetOrderDetailsById(existingAccount);
            }
        }

        public IActionResult OnPostConfirmedReceipt(string orderDetailId)
        {
            bool isUpdated = _orderDetailService.UpdateOrderStatusByOrderDetailId(orderDetailId, 3);

            if (isUpdated)
            {
                TempData["SuccessConfirmedReceiptMessage"] = "Xác nh?n ?ã nh?n hàng";
                return RedirectToPage();

            }
            TempData["ErrorMessage"] = "Không th? c?p nh?t tr?ng thái. Vui lòng th? l?i.";
            return RedirectToPage();

        }

        public IActionResult OnPostCancelStatus(string orderDetailId)
        {
            bool isUpdated = _orderDetailService.UpdateOrderStatusByOrderDetailId(orderDetailId, 4);

            if (isUpdated)
            {
                TempData["SuccessCancelMessage"] = "H?y ??n hàng thành công";
                return RedirectToPage();

            }
            TempData["ErrorMessage"] = "Không th? c?p nh?t tr?ng thái. Vui lòng th? l?i.";
            return RedirectToPage();

        }
    }
}
