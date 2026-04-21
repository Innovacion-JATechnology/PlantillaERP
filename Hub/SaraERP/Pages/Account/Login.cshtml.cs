using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using UserRoles.Identity.Models;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Users> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El correo electrónico es requerido")]
            [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
            [Display(Name = "Correo Electrónico")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Recordarme")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Validar ReturnUrl - solo aceptar URLs locales y seguras
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/Hub/Hubindex";
            }
            // Si el ReturnUrl contiene problemas, también usar Hub/Hubindex
            else if (returnUrl.Contains("?") || returnUrl.Contains("&"))
            {
                returnUrl = "/Hub/Hubindex";
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Validar ReturnUrl - solo aceptar URLs locales y seguras
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/Hub/Hubindex";
            }
            // Si el ReturnUrl contiene problemas, también usar Hub/Hubindex
            else if (returnUrl.Contains("?") || returnUrl.Contains("&"))
            {
                returnUrl = "/Hub/Hubindex";
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Buscar el usuario por email primero
                var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                if (user != null)
                {
                    // Autenticar usando el nombre de usuario encontrado
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        Input.Password,
                        Input.RememberMe,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Usuario {Email} logueado exitosamente", Input.Email);
                        // Redirigir a /Hub/Hubindex
                        return Redirect("/Hub/Hubindex");
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("Cuenta bloqueada para usuario {Email}", Input.Email);
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos.");
                        _logger.LogWarning("Intento de login fallido para usuario {Email}", Input.Email);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos.");
                    _logger.LogWarning("Usuario no encontrado para email {Email}", Input.Email);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}