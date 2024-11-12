using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Blossom_BusinessObjects;

namespace Blossom_RazorWeb.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        private readonly IFeedbackService _feedbackService;
        public Flower Flower { get; set; }
        public Account SellerInfo { get; set; }
        public int Quantity { get; set; } = 1; // Default cart quantity
        public List<Feedback> ListFeedback { get; set; }

        [BindProperty]
        public Feedback NewFeedback { get; set; }

        //Seller
        public decimal SellerRatingAverage;
        public int SellerRatingCount;
        public int SellerProductCount;

        public DetailsModel(IFlowerService flowerService, IFeedbackService feedbackService)
        {
            _flowerService = flowerService;
            _feedbackService = feedbackService;
            SellerRatingAverage = 5;
            SellerRatingCount = 3;
            SellerProductCount = 10;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Flower = await _flowerService.GetFlower(id);
            ListFeedback = await _feedbackService.GetFeedbackByFlowerIdAsync(id);

            SellerInfo = Flower.Seller;

            if (Flower == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}