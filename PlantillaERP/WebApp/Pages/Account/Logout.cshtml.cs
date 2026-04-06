using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserRoles.Identity.Models;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<Users> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Con Opción A: el GET no se usa, todo es POST directo desde JavaScript
            // Si alguien intenta acceder por GET, redirigir al home
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPost()
        {
            _logger.LogInformation("🔓 Iniciando logout para usuario: {UserName}", User?.Identity?.Name ?? "Unknown");

            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    await _signInManager.SignOutAsync();
                    _logger.LogInformation("✅ Usuario {UserName} cerró sesión correctamente", User?.Identity?.Name ?? "Unknown");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al cerrar sesión del usuario: {UserName}", User?.Identity?.Name ?? "Unknown");
            }

            // Redirigir a la página de éxito
            return RedirectToPage("/Account/LogoutSuccess");
        }
    }
}
