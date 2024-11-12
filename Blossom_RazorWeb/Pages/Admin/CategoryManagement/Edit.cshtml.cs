using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.CategoryManagement
{
    public class EditModel : PageModel
    {
        private readonly IFlowerCategoryService _flowerCategoryService;

        [BindProperty]
        public FlowerCategory FlowerCategory { get; set; }

        public EditModel(IFlowerCategoryService flowerCategoryService)
        {
            _flowerCategoryService = flowerCategoryService;
        }

        // GET request
        public async Task<IActionResult> OnGetAsync(string id)
        {
            FlowerCategory = await _flowerCategoryService.GetFlowerCategory(id);
            if (FlowerCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        // POST request
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _flowerCategoryService.UpdateFlowerCategory(FlowerCategory);

            return RedirectToPage("./Index");
        }
    }
}
