using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerChannel.FlowerManagement
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerService _flowerService;

        public IndexModel(ILogger<IndexModel> logger, IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        public List<Flower> Flowers { get; set; }

        public void OnGet()
        {
            var accountId = HttpContext.Session.GetString("AccountId");

            if (accountId != null)
            {
                Flowers = _flowerService.GetFlowersBySeller(accountId).Result;
            }

        }

        public string GetStatusColor(FlowerStatus status)
        {
            return status switch
            {
                FlowerStatus.PENDING => "text-warning", // Vàng
                FlowerStatus.APPROVED => "text-success", // Xanh lá
                FlowerStatus.REJECTED => "text-danger", // Đỏ
                FlowerStatus.EXPIRED => "text-muted", // Xám
                _ => "text-dark" // Mặc định
            };
        }

    }
}
