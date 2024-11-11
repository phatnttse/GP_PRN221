using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.CategoryManagement
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerCategoryService _flowerCategoryService;

        public IEnumerable<FlowerCategory> FlowerCategories { get; set; }

        public IndexModel(IFlowerCategoryService flowerCategoryService)
        {
            _flowerCategoryService = flowerCategoryService;
        }

        public async Task OnGetAsync()
        {
            FlowerCategories = await _flowerCategoryService.GetFlowerCategories();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _flowerCategoryService.DeleteFlowerCategory(id);
            return RedirectToPage("./Index");
        }
    }
}
