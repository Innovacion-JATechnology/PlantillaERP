using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserRoles.Identity.Models;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class LogoutSuccessModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;

        public LogoutSuccessModel(SignInManager<Users> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
  