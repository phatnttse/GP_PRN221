using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Blossom_RazorWeb.Pages.SellerChannel.FlowerManagement
{
    public class CreateModel : PageModel
    {
        private readonly IFlowerService _flowerService;
        private readonly IFlowerCategoryService _flowerCategoryService;

        public CreateModel(IFlowerService flowerService, IFlowerCategoryService flowerCategoryService)
        {
            _flowerService = flowerService;
            _flowerCategoryService = flowerCategoryService;
        }

        [BindProperty]
        public Flower NewFlower { get; set; }

        [BindProperty]
        public IFormFile UploadImage { get; set; }

        public string ErrorMessage { get; private set; }

        public List<FlowerCategory> FlowerCategories { get; set; } = new List<FlowerCategory>();

        public void OnGet()
        {
            FlowerCategories = _flowerCategoryService.GetFlowerCategories().Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var accountId = HttpContext.Session.GetString("AccountId");
                if (accountId != null)
                NewFlower.SellerId = accountId;

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
                    NewFlower.ImageUrl = $"/assets/images/{fileName}";
                }


                NewFlower.Id = Guid.NewGuid().ToString();
                NewFlower.Status = FlowerStatus.PENDING;
                NewFlower.Views = 0;

                var result = await _flowerService.AddFlower(NewFlower);

                if (result)
                {
                    TempData["SuccessMessage"] = "Tạo hoa mới thành công!";

                    return RedirectToPage("/SellerChannel/FlowerManagement/Index");

                }

                return RedirectToPage("/SellerChannel/FlowerManagement/Create");

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

                
        }
    }
}
