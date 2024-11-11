using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.CategoryManagement
{
    public class CreateModel : PageModel
    {
        private readonly IFlowerCategoryService _flowerCategoryService;

        [BindProperty]
        public FlowerCategory FlowerCategory { get; set; }

        public CreateModel(IFlowerCategoryService flowerCategoryService)
        {
            _flowerCategoryService = flowerCategoryService;
        }

        // GET request
        public void OnGet()
        {
        }

        // POST request
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _flowerCategoryService.AddFlowerCategory(FlowerCategory);

            return RedirectToPage("./Index");
        }
    }
}
