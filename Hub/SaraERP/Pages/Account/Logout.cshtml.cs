using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class LogoutSuccessModel : PageModel
    {
        public void OnGet()
        {
            // Esta página solo muestra el mensaje de sesión cerrada
            // No necesita hacer nada especial en OnGet
        }
    }
}
