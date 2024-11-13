using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerChannel.OrderManagement
{
    public class IndexModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUserIdAssessor _userIdAssessor;
        public IndexModel(IOrderDetailService orderDetailService, IUserIdAssessor userIdAssessor)
        {
            _orderDetailService = orderDetailService;   
            _userIdAssessor = userIdAssessor;
        }
        public List<OrderDetail> OrderDetails { get; set; } 
        public void OnGet()
        {
            var existingAccount = _userIdAssessor.GetCurrentUserId();
            if (existingAccount != null)
            {
                OrderDetails = _orderDetailService.GetOrderDetailsBySellerId(existingAccount);
            }
        }
    }
}
