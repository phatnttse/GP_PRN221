using Blossom_BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blossom_RazorWeb.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Account> _userManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(UserManager<Account> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public IFormFile AvatarFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Account = await _userManager.GetUserAsync(User);
            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = Account.FullName;
            user.Gender = Account.Gender;
            user.Address = Account.Address;

            if (AvatarFile != null)
            {
                var fileName = $"{Guid.NewGuid()}_{AvatarFile.FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }
                user.Avatar = $"/assets/images/{fileName}";           
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var account = await _userManager.GetUserAsync(User);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                HttpContext.Session.Remove("Avatar");
                HttpContext.Session.SetString("Avatar", account.Avatar);
                return Page();
            }

            return RedirectToPage();
        }
    }
}
