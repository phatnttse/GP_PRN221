using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Blossom_BusinessObjects;
using Blossom_Services;
using Blossom_RazorWeb.ViewModels;

namespace Blossom_RazorWeb.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        private readonly IFeedbackService _feedbackService;
        private readonly ICartItemService _cartItemService;
        private readonly IUserIdAssessor _userIdAssessor;
        public Flower Flower { get; set; }
        public Account SellerInfo { get; set; }
        public List<Feedback> ListFeedback { get; set; }

        [BindProperty]
        public FeedbackViewModel NewFeedback { get; set; }

        //Seller
        public decimal SellerRatingAverage;
        public int SellerRatingCount;
        public int SellerProductCount;

        public string ErrorMessage { get; set; }


        public DetailsModel(IFlowerService flowerService, IFeedbackService feedbackService, IUserIdAssessor userIdAssessor)
        {
            _flowerService = flowerService;
            _feedbackService = feedbackService;
            _userIdAssessor = userIdAssessor;
            SellerRatingAverage = 5;
            SellerRatingCount = 3;
            SellerProductCount = 10;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Flower = await _flowerService.GetFlower(id);

            if (Flower == null)
            {
                return NotFound();
            }

            await _flowerService.IncrementViews(id);
            ListFeedback = await _feedbackService.GetFeedbackByFlowerIdAsync(id);

            SellerInfo = Flower.Seller;

            return Page();
        }

        public async Task<IActionResult> OnPostBuyNow(string flowerId)
        {
            try
            {
                var currentUserId = _userIdAssessor.GetCurrentUserId();
                var submittedQuantity = Request.Form["Quantity"];

                // Chuyển đổi từ chuỗi sang số nguyên nếu cần
                int quantity = int.TryParse(submittedQuantity, out var result) ? result : 0;

                if (currentUserId != null)
                {
                    var cartItem = await _cartItemService.GetByUserAndFlowerAsync(currentUserId, flowerId);
                    var flowerQuantity = await _flowerService.GetFlower(flowerId);
                    if (cartItem != null)
                    {
                        if (flowerQuantity.StockQuantity < cartItem.Quantity + quantity)
                        {
                            cartItem.Quantity = flowerQuantity.StockQuantity;
                            TempData["ErrorMessage"] = "Số lượng sản phẩm trong giỏ đã vượt quá số lượng có sẵn";
                            return RedirectToPage();
                        }

                        await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, quantity);
                        return RedirectToPage("/Order");

                    }
                    else
                    {
                        await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, quantity);
                        return RedirectToPage("/Order");
                    }

                }
                else
                {
                    TempData["NotPermissionMessage"] = "Vui lòng đăng nhập!";
                    return RedirectToPage("/Auth/Login");
                }

            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostAddToCart(string flowerId)
        {
            try
            {
                var currentUserId = _userIdAssessor.GetCurrentUserId();
                var submittedQuantity = Request.Form["Quantity"];

                // Chuyển đổi từ chuỗi sang số nguyên nếu cần
                int quantity = int.TryParse(submittedQuantity, out var result) ? result : 0;

                if (currentUserId != null)
                {
                    var cartItem = await _cartItemService.GetByUserAndFlowerAsync(currentUserId, flowerId);
                    var flowerQuantity = await _flowerService.GetFlower(flowerId);
                    if (cartItem != null)
                    {
                        if (flowerQuantity.StockQuantity < cartItem.Quantity + quantity)
                        {
                            cartItem.Quantity = flowerQuantity.StockQuantity;
                            TempData["ErrorMessage"] = "Số lượng sản phẩm trong giỏ đã vượt quá số lượng có sẵn";
                            return RedirectToPage();
                        }

                        await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, quantity);
                        return RedirectToPage("/CartItem");

                    }
                    else
                    {
                        await _cartItemService.AddFlowerListingToCart(currentUserId, flowerId, quantity);
                        return RedirectToPage("/CartItem");
                    }

                }
                else
                {
                    TempData["NotPermissionMessage"] = "Vui lòng đăng nhập!";
                    return RedirectToPage("/Auth/Login");
                }

            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }
        } 

        public async Task<IActionResult> OnPostSubmitFeedbackAsync(string id)
        {
            var userId = _userIdAssessor.GetCurrentUserId();
            if (userId == null)
            {
                TempData["NotPermissionMessage"] = "Authentication failed, please login to continue!";
                return Redirect("/Auth/Login");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToPage(new { id });
            }

            // Populate the feedback object with additional data
            Feedback feedback = new Feedback()
            {
                FlowerId = id,
                UserId = _userIdAssessor.GetCurrentUserId(),
                Rating = NewFeedback.Rating,
                Description = NewFeedback.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Save feedback to the database
            await _feedbackService.AddFeedbackAsync(feedback);

            // Reload feedback list to reflect the new feedback
            ListFeedback = await _feedbackService.GetFeedbackByFlowerIdAsync(id);

            TempData["Message"] = "Thank you for your feedback!";
            return RedirectToPage(new { id });
        }
    }
}