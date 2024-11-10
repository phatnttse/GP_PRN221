using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFlowerService _flowerService;

        public IndexModel(ILogger<IndexModel> logger, IFlowerService flowerService)
        {
            _logger = logger;
            _flowerService = flowerService;
        }

        public List<Flower> Flowers { get; set; }

        public void OnGet()
        {
            Flowers = _flowerService.GetFlowers().Result;
        }

    }
}
