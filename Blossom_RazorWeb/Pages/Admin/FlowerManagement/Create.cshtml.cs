using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.FlowerManagement
{
    public class CreateModel : PageModel
    {
        private readonly IFlowerService _flowerService;

        [BindProperty]
        public Flower Flower { get; set; }

        public IEnumerable<Account> Sellers { get; set; }
        public IEnumerable<FlowerCategory> FlowerCategories { get; set; }


        public CreateModel(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            //Sellers = await _flowerService.GetAllSellersAsync();  
            //FlowerCategories = await _flowerService.GetAllFlowerCategoriesAsync(); 

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            //if (!ModelState.IsValid)
            //{

            //    Sellers = await _flowerService.GetAllSellersAsync();
            //    FlowerCategories = await _flowerService.GetAllFlowerCategoriesAsync();
            //    return Page();
            //}


            //await _flowerService.CreateFlowerAsync(Flower);


            return RedirectToPage("./Index");
        }
    }
}
