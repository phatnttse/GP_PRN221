using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.FlowerManagement
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerService flowerService;

        public IndexModel(IFlowerRepository flowerRepository)
        {
            flowerService = new FlowerService(flowerRepository);
        }

        public IList<Flower> Flowers { get; set; }

        public async Task OnGetAsync()
        {
            Flowers = await flowerService.GetAdminFlowers();
        }
    }
}
