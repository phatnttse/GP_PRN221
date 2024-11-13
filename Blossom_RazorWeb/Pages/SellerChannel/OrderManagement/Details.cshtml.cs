using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerChannel.OrderManagement
{
    public class DetailsModel : PageModel
    {

        private IOrderDetailService _orderDetailService;
        public DetailsModel(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public OrderDetail OrderDetail { get; set; }
        public void OnGet(string id)
        {
            OrderDetail = _orderDetailService.GetOrderDetail(id);
        }

        public IActionResult OnPostUpdateStatus(string orderDetailId)
        {
            bool isUpdated = _orderDetailService.UpdateOrderStatusByOrderDetailId(orderDetailId, 1);

            if (isUpdated)
            {
                TempData["SuccessStatusMessage"] = "Trạng thái đơn hàng đã được cập nhật.";
                return RedirectToPage("/SellerChannel/OrderManagement/Index");

            }
                TempData["ErrorMessage"] = "Không thể cập nhật trạng thái. Vui lòng thử lại.";
            return Page();

        }

        public IActionResult OnPostCancelStatus(string orderDetailId)
        {
            bool isUpdated = _orderDetailService.UpdateOrderStatusByOrderDetailId(orderDetailId, 4);

            if (isUpdated)
            {
                TempData["SuccessCancelMessage"] = "Hủy đơn hàng thành công";
                return RedirectToPage("/SellerChannel/OrderManagement/Index"); 

            }
            TempData["ErrorMessage"] = "Không thể cập nhật trạng thái. Vui lòng thử lại.";
            return Page();

        }
    }
}
