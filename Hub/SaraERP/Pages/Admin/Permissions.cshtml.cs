using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Models;

namespace WebApp.Pages.Admin
{
    [Authorize(Roles = SystemRoles.Admin)]
    public class PermissionsModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public List<IdentityRole> Roles { get; set; }
        public List<Users> Users { get; set; }

        public PermissionsModel(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
        }

        public async Task<IActionResult> OnPostAsync(string userId, string roleId, string action)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user != null && role != null)
            {
                if (action == "assign")
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (action == "remove")
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            return RedirectToPage();
        }
    }
}
