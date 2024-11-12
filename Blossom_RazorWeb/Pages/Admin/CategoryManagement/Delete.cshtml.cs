using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.CategoryManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IFlowerCategoryService _flowerCategoryService;

        [BindProperty]
        public FlowerCategory FlowerCategory { get; set; }

        public DeleteModel(IFlowerCategoryService flowerCategoryService)
        {
            _flowerCategoryService = flowerCategoryService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            FlowerCategory = await _flowerCategoryService.GetFlowerCategory(id);

            if (FlowerCategory == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FlowerCategory == null)
            {
                return NotFound();
            }

            await _flowerCategoryService.DeleteFlowerCategory(FlowerCategory.Id);

            return RedirectToPage("./Index"); 
        }
    }
}
