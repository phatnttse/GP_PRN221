using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.SellerChannel.FlowerManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IFlowerService _flowerService;

        public DeleteModel(IFlowerService flowerService, IFlowerCategoryService flowerCategoryService)
        {
            _flowerService = flowerService;
        }

        [BindProperty]
        public Flower Flower { get; set; }

        public string ErrorMessage { get; private set; }

        public void OnGet(string id)
        {
            Flower = _flowerService.GetFlower(id).Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _flowerService.DeleteFlower(Flower.Id);

                if (result)
                return RedirectToPage("/SellerChannel/FlowerManagement/Index");

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
