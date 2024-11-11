using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;

namespace Blossom_RazorWeb.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        public Flower Flower { get; set; }
        public Account SellerInfo { get; set; }
        public int Quantity { get; set; } = 1; // Default quantity

        public DetailsModel(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Flower = await _flowerService.GetFlower(id);

            SellerInfo = Flower.Seller;

            if (Flower == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}