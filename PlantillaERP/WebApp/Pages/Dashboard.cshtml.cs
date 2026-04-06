using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserRoles.Identity.Models;

namespace WebApp.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;

        public DashboardModel(SignInManager<Users> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
