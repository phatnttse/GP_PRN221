using Blossom_BusinessObjects.Entities.Enums;
using Blossom_BusinessObjects.Entities;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Admin.FlowerManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IFlowerService _flowerService;

        public DetailsModel(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [BindProperty]
        public Flower Flower { get; set; }

        [BindProperty]
        public string RejectReason { get; set; }

        [BindProperty]
        public string FlowerId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Flower = await _flowerService.GetFlower(id);
            if (Flower == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(string id)
        {
            var flower = await _flowerService.GetFlower(FlowerId);
            if (flower == null)
            {
                return NotFound();
            }

            flower.Status = FlowerStatus.APPROVED;
            await _flowerService.UpdateFlower(flower);

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRejectAsync(string id)
        {
            var flower = await _flowerService.GetFlower(FlowerId);
            if (flower == null)
            {
                return NotFound();
            }

            flower.Status = FlowerStatus.REJECTED;
            flower.RejectReason = RejectReason;
            await _flowerService.UpdateFlower(flower);

            return RedirectToPage("./Index");
        }
    }
}
