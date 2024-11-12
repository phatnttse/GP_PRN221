using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerChannel.FlowerManagement
{
    public class EditModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        private readonly IFlowerCategoryService _flowerCategoryService;

        public EditModel(IFlowerService flowerService, IFlowerCategoryService flowerCategoryService)
        {
            _flowerService = flowerService;
            _flowerCategoryService = flowerCategoryService;
        }

        [BindProperty]
        public Flower Flower { get; set; }

        [BindProperty]
        public IFormFile? UploadImage { get; set; }

        public string ErrorMessage { get; private set; }

        public List<FlowerCategory> FlowerCategories { get; set; } = new List<FlowerCategory>();

        public void OnGet(string id)
        {
            Flower = _flowerService.GetFlower(id).Result;
            FlowerCategories = _flowerCategoryService.GetFlowerCategories().Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var accountId = HttpContext.Session.GetString("AccountId");
                if (accountId != null)
                    Flower.SellerId = accountId;

                if (UploadImage != null)
                {
                    // Tạo tên file ảnh duy nhất
                    var fileName = $"{Guid.NewGuid()}_{UploadImage.FileName}";

                    // Đường dẫn đầy đủ đến thư mục assets/images
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images", fileName);

                    // Lưu ảnh vào thư mục assets/images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadImage.CopyToAsync(stream);
                    }

                    // Lưu URL ảnh vào thuộc tính ImageUrl
                    Flower.ImageUrl = $"/assets/images/{fileName}";
                }

                var result = await _flowerService.UpdateFlower(Flower);

                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công!";

                    return RedirectToPage("/SellerChannel/FlowerManagement/Index");

                }

                return Page();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();

            }


        }
    }
}
