using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerProfile
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        private readonly IAccountService _accountService;

        public IndexModel(IFlowerService flowerService, IAccountService accountService)
        {
            _flowerService = flowerService;
            _accountService = accountService;
        }

        public Account SellerInfo { get; set; }
        public List<Flower> FlowerList { get; set; } = new List<Flower>();
        public string SellerId { get; set; } = "defaultSellerId"; // Replace with dynamic value

        public async Task<IActionResult> OnGetAsync(string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return RedirectToPage("/Index");
            }

            SellerId = sellerId;

            if (!string.IsNullOrEmpty(SellerId))
            {
                SellerInfo = await _accountService.GetAccountById(SellerId);
                FlowerList = await _flowerService.GetFlowersBySeller(SellerId);
            }

            if (SellerInfo == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
